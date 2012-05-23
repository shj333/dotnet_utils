/*
 * Copyright 2012 Berwick Heights Software, Inc
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in 
 * compliance with the License. You may obtain a copy of the License at 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under the License is 
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and limitations under the License.
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.Logging;
using BerwickHeights.Platform.PerfTest.Attrib;
using BerwickHeights.Platform.PerfTest.Model;
using BerwickHeights.Platform.PerfTest.Svc;

namespace BerwickHeights.Platform.PerfTest.Test
{
    /// <inheritDoc/>
    public class PerfTestController : IPerfTestController
    {
        #region Private Fields

        private readonly IPerfTestSvc perfTestSvc;
        private readonly ICurrentUserSvc currentUserSvc;
        private readonly ILogger logger;
        private readonly IDictionary<string, TestSuiteResult> resultsMap;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PerfTestController(IPerfTestSvc perfTestSvc, ICurrentUserSvc currentUserSvc,
            ILoggerFactory loggerFactory)
        {
            this.perfTestSvc = perfTestSvc;
            this.currentUserSvc = currentUserSvc;
            logger = loggerFactory.GetLogger(GetType());
            resultsMap = new Dictionary<string, TestSuiteResult>();
        }

        #endregion

        #region IPerfTestController Implementation

        /// <inheritDoc/>
        public TestSuiteResult RunPerfTestsInAssemblies(string annotation, params string[] assemblyNames)
        {
            if ((assemblyNames == null) || (!assemblyNames.Any()))
            {
                throw new ArgumentException("Parameter 'assemblyNames' cannot be empty");
            }

            // Set up test result data
            TestSuiteResult testSuiteResult = CreateTestSuiteResult(annotation);

            // Loop through given assemblies and run performance tests in them
            foreach (string assemblyName in assemblyNames)
            {
                try
                {
                    RunTestsInAssembly(assemblyName, testSuiteResult);
                }
                catch (Exception e)
                {
                    logger.Error("Caught exception in running performance tests in assembly: "
                        + assemblyName, e);
                }
            }

            // Test suite is now done -- set end time and persist the results
            testSuiteResult.EndTime = DateTime.Now;
            SaveTestSuiteResult(testSuiteResult);

            // Return test run results
            return testSuiteResult;
        }

        /// <inheritDoc/>
        public void BeginPerfTest(string testId, string annotation, string typeName, string methodName)
        {
            // Add current user ID to test ID to differentiate results among parallel users
            testId = AddCurrentUserToTestId(testId);

            // See if we already have a test result for given ID
            TestSuiteResult testSuiteResult;
            lock (resultsMap)
            {
                if (resultsMap.ContainsKey(testId))
                {
                    testSuiteResult = resultsMap[testId];
                }
                else
                {
                    // Create new object for test results and add it to results map
                    testSuiteResult = CreateTestSuiteResult(annotation);
                    resultsMap[testId] = testSuiteResult;
                }
            }

            // Set up new test
            SetupCurrentTest(testSuiteResult, annotation, typeName, methodName);
        }

        /// <inheritDoc/>
        public void EndPerfTest(string testId)
        {
            // Make sure there is a current test running on this thread
            TestResult currentTestResult = perfTestSvc.GetCurrentTestResult();
            if (currentTestResult == null) return;

            // Clear out current test result
            ClearCurrentTest(currentTestResult);

            // Add current user ID to test ID to differentiate results among parallel users
            testId = AddCurrentUserToTestId(testId);

            lock (resultsMap)
            {
                // Make sure given test ID matches a result object in map
                if (!resultsMap.ContainsKey(testId)) return;

                // Get result data from map and update end time of entire test suite
                TestSuiteResult testSuiteResult = resultsMap[testId];
                testSuiteResult.EndTime = DateTime.Now;
            }
        }

        /// <inheritDoc/>
        public void SavePerfTestResults(string testId)
        {
            // Add current user ID to test ID to differentiate results among parallel users
            testId = AddCurrentUserToTestId(testId);

            TestSuiteResult testSuiteResult;
            lock (resultsMap)
            {
                // Make sure given test ID matches a result object in map
                if (!resultsMap.ContainsKey(testId)) return;

                // Get result data from map and persist results
                testSuiteResult = resultsMap[testId];
            }

            // Persist results in data store
            SaveTestSuiteResult(testSuiteResult);
        }

        /// <inheritDoc/>
        public void ClearPerfTestResults(string testId)
        {
            // Add current user ID to test ID to differentiate results among parallel users
            testId = AddCurrentUserToTestId(testId);

            lock (resultsMap)
            {
                if (!resultsMap.ContainsKey(testId)) return;
                resultsMap.Remove(testId);
            }
        }

        #endregion

        #region Private Methods

        private string AddCurrentUserToTestId(string testId)
        {
            // Get current user ID for this thread
            string userId = currentUserSvc.GetCurrentUserData().UserId;
            if (userId == null)
            {
                throw new Exception("User ID not set for performance test: " + testId);
            }

            // Add current user ID in order to differentiate concurrent users in perf test results
            return testId + "-" + userId;
        }

        private TestSuiteResult CreateTestSuiteResult(string annotation)
        {
            // Create object to hold test run results
            TestSuiteResult testSuiteResult = new TestSuiteResult(currentUserSvc.GetCurrentUserData().UserId);

            // Set user-supplied annotation for test run
            testSuiteResult.Annotation = annotation;

            // Set system info for this test run
            SetSystemInfo(testSuiteResult);

            // Set start time after getting system info so it doesn't affect results
            testSuiteResult.StartTime = DateTime.Now;

            return testSuiteResult;
        }

        private void SaveTestSuiteResult(TestSuiteResult testSuiteResult)
        {
            //
            // Put mutex on this instance of test suite results in case multiple threads 
            // are trying to persist it
            //
            lock (testSuiteResult)
            {
                perfTestSvc.SaveTestResults(testSuiteResult);
            }
        }

        private TestResult SetupCurrentTest(TestSuiteResult testSuiteResult, string annotation,
            string typeName, string methodName)
        {
            // Add a new test data instance to test run results for this perf test
            TestResult testResult = new TestResult(typeName, methodName);
            testResult.Annotation = annotation;

            //
            // Put mutex on this instance of test suite results in case multiple threads 
            // are trying to access it
            //
            lock (testSuiteResult)
            {
                testSuiteResult.TestResultList.Add(testResult);
            }

            //
            // Set current test data results object in thread local storage so it 
            // can be accessed anywhere in stack
            //
            perfTestSvc.SetCurrentTestResult(testResult);
            return testResult;
        }

        private void ClearCurrentTest(PerfTestBase testResult, DateTime endTime = default(DateTime))
        {
            if (testResult != null)
            {
                // Set end time for this test
                testResult.EndTime = (endTime == default(DateTime)) ? DateTime.Now : endTime;
            }

            // Clear out the thread local storage reference to test data
            perfTestSvc.SetCurrentTestResult(null);
        }

        private void RunTestsInAssembly(string assemblyName, TestSuiteResult testSuiteResult)
        {
            // Load given assembly
            Assembly assembly = Assembly.Load(assemblyName);

            // Pass empty object to performance test methods
            object[] parms = new object[] { };

            // Loop through non-abstract types in given assembly
            foreach (Type type in assembly.GetTypes().Where(type => !type.IsAbstract))
            {
                // Create instance of type
                object instance;
                try
                {
                    instance = Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    logger.Error("Caught exception in creating type " + type
                        + " in assembly " + assemblyName, e);

                    // Continue on to next type
                    continue;
                }

                // Make sure instance implements required interface
                IPerfTestApp perfTestApp = instance as IPerfTestApp;
                if (perfTestApp == null)
                {
                    logger.Warn("Type does not support required interface: " + type
                        + " in assembly " + assemblyName);

                    // Continue on to next type
                    continue;
                }

                // Run setup method
                try
                {
                    perfTestApp.Setup();
                }
                catch (Exception e)
                {
                    logger.Error("Caught exception in running setup for " + type
                        + " in assembly " + assemblyName, e);

                    // Continue on to next type
                    continue;
                }

                // Run all performance tests
                foreach (MethodInfo method in type.GetMethods().Where(
                    method => HasAttribute(method, typeof(PerfTestAttribute))))
                {
                    //
                    // Create new test result data and set in thread local storage so it 
                    // can be accessed anywhere in stack
                    //
                    DateTime endTime = default(DateTime);
                    TestResult testResult = SetupCurrentTest(testSuiteResult, string.Empty,
                        type.Name, method.Name);

                    try
                    {
                        // Run performance test for this method
                        method.Invoke(instance, parms);

                        // Gather end time before running stats so it doesn't affect perf time
                        endTime = DateTime.Now;

                        // Gather stats
                        try
                        {
                            testResult.Annotation = (string.IsNullOrEmpty(testResult.Annotation)) ? "" : ", ";
                            testResult.Annotation += perfTestApp.GetStats();
                        }
                        catch (Exception e)
                        {
                            logger.Error("Caught exception in gathering stats for " + type
                                + " in assembly " + assemblyName, e);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Error("Caught exception in running perf test for " + type
                            + " in assembly " + assemblyName, e);

                        // Test failed
                        testResult.IsSuccess = false;
                    }
                    finally
                    {
                        ClearCurrentTest(testResult, endTime);
                    }
                }

                // Run tear down method
                try
                {
                    perfTestApp.TearDown();
                }
                catch (Exception e)
                {
                    logger.Error("Caught exception in running tear down for " + type
                        + " in assembly " + assemblyName, e);
                }
            }
        }

        private static bool HasAttribute(MemberInfo member, Type attrib)
        {
            return member.GetCustomAttributes(true).Any(
                attrib2 => attrib2.GetType().IsEquivalentTo(attrib));
        }

        private static void SetSystemInfo(TestSuiteResult testSuiteResult)
        {
            const string componentCpu = "CPU";
            const string componentRam = "RAM";
            IDictionary<string, IDictionary<string, string[]>> systemInfoKeys =
                new Dictionary<string, IDictionary<string, string[]>>();
            systemInfoKeys["Win32_ComputerSystem"] = new Dictionary<string, string[]>
            {
                { componentRam, new string[] { "TotalPhysicalMemory" } }
            };
            systemInfoKeys["Win32_Processor"] = new Dictionary<string, string[]>
            {
                { componentCpu, new string[] {"Manufacturer","AddressWidth","Description","Name","MaxClockSpeed"} },
            };
            systemInfoKeys["Win32_PerfFormattedData_PerfOS_Processor"] = new Dictionary<string, string[]>
            {
                { componentCpu, new string[] {"PercentIdleTime"} },
            };

            foreach (string key in systemInfoKeys.Keys)
            {
                IDictionary<string, string[]> wmiKeys = systemInfoKeys[key];
                ManagementObjectSearcher search = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject wmiObj in search.Get())
                {
                    foreach (string key2 in wmiKeys.Keys)
                    {
                        foreach (string key3 in wmiKeys[key2])
                        {
                            try
                            {
                                testSuiteResult.SystemInfoList.Add(new SystemInfo(key2, key3,
                                    wmiObj[key3].ToString()));
                            }
                            catch
                            {
                                // Ignore exception -- probably means data is not available
                            }
                        }
                    }
                }
            }

            // Get current CPU utilization
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            testSuiteResult.SystemInfoList.Add(new SystemInfo(componentCpu, "CPU Usage",
                cpuCounter.NextValue() + "%"));

            // Get current available RAM
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            testSuiteResult.SystemInfoList.Add(new SystemInfo("RAM", "Available RAM",
                ramCounter.NextValue() + "MB"));
        }

        #endregion
    }
}
