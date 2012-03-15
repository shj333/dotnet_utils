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

using System.Collections.Generic;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.PerfTest.Model;

namespace BerwickHeights.Platform.PerfTest.Test
{
    /// <summary>
    /// Controller for running performance tests. 
    /// </summary>
    public interface IPerfTestController : IIoCComponent
    {
        /// <summary>
        /// Runs all performance tests found in the given list of assemblies. Uses reflection
        /// to find any classes in the given list of assemblies that have methods marked with
        /// the "PerfTest" method attribute. Each class can also mark a setup and
        /// tear down method as well with the "PerfTestSetup" and "PerfTestTearDown"
        /// method attributes. Each class should contain only one setup and one tear down
        /// method. This method returns the resultant test run data, which is also stored 
        /// in the data store.
        /// </summary>
        /// <param name="assemblyNames">The list of assemblies to inspect for performance tests
        /// to run.</param>
        /// <param name="annotation">The user-supplied annotation for this test run.</param>
        /// <returns>The resultant test run data, which is also stored in the data store.</returns>
        TestSuiteResult RunPerfTestsInAssemblies(IEnumerable<string> assemblyNames, string annotation);

        /// <summary>
        /// Sets up a performance test so that results can be kept for the test. All results from calls 
        /// to this method with the same testID are associated with a single test suite result record
        /// in the data store. Call EndPerfTest() when this performance test concludes so that
        /// results can be persisted to data store.
        /// </summary>
        /// <param name="testId">Application-defined, unique ID for the current suite of tests. Used to
        /// aggregate test suite results into a single record in data store.</param>
        /// <param name="annotation">Application-defined annotation for the test suite.</param>
        /// <param name="typeName">The application-defined name of the type that generated this 
        /// performance test.</param>
        /// <param name="methodName">The application-defined name of the method that generated this 
        /// performance test.</param>
        void BeginPerfTest(string testId, string annotation, string typeName, string methodName);

        /// <summary>
        /// Marks the end of a performance test. Call BeginPerfTest() at the beginning of the 
        /// performance test to set up the test result data.
        /// </summary>
        /// <param name="testId">Application-defined, unique ID for the current suite of tests. 
        /// Used to aggregate test suite results into a single record in data store.</param>
        void EndPerfTest(string testId);

        /// <summary>
        /// Persists all test results for the given test ID in the data store. The results for 
        /// all calls to BeginPerfTest() and EndPerfTest() for the given testID are aggregated
        /// into a suite of test results and persisted.
        /// </summary>
        /// <param name="testId">Application-defined, unique ID for the current suite of tests. 
        /// Used to aggregate test suite results into a single record in data store.</param>
        void SavePerfTestResults(string testId);

        /// <summary>
        /// Clears the performance test suite with the given test ID from memory. If a subsequent
        /// call to BeginPerfTest() is made with this test ID, then a new result set is
        /// created.
        /// </summary>
        /// <param name="testId">Application-defined, unique ID for the current suite of tests. 
        /// Used to aggregate test suite results into a single record in data store.</param>
        void ClearPerfTestResults(string testId);
    }
}
