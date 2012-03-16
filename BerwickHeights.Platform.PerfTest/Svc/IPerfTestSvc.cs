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
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.PerfTest.Model;

namespace BerwickHeights.Platform.PerfTest.Svc
{
    /// <summary>
    /// This service provides persistence of performance test results data.
    /// </summary>
    public interface IPerfTestSvc : IIoCComponent
    {
        /// <summary>
        /// Create or update the record for the given performance test results in data store. 
        /// If the TestSuiteResultId property is set in the given data, then the record is
        /// updated in the data store; otherwise, a new record is created in the data store.
        /// All data associated with the test suite results (e.g., test results, system 
        /// information, timing data) are cascade persisted.
        /// </summary>
        /// <param name="testSuiteResult">The performance results to store.</param>
        void SaveTestResults(TestSuiteResult testSuiteResult);

        /// <summary>
        /// Retrieve given performance test results from data store.
        /// </summary>
        /// <param name="testSuiteResultId">The unique ID of the performance test results to retrieve.</param>
        /// <returns>The matching performance test results, or <code>null</code> if no matching
        /// record is found.</returns>
        TestSuiteResult GetTestResults(string testSuiteResultId);

        /// <summary>
        /// Find performance test results for tests that started between the two given dates.
        /// </summary>
        /// <param name="startTime">The earliest time that a performance test started.</param>
        /// <param name="endTime">The latest time that a performance test started.</param>
        /// <returns>The list of matching performance test results, or an empty list if
        /// no matching records are found.</returns>
        IEnumerable<TestSuiteResult> GetTestResults(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Retrieves the current performance test results from the thread local storage for 
        /// this thread. By using thread local storage, the current performance test results 
        /// can be retrieved at any point in the architecture stack.
        /// </summary>
        TestResult GetCurrentTestResult();

        /// <summary>
        /// Saves the given performance test result in the thread local storage for this
        /// thread. By using thread local storage, the current performance test results 
        /// can be retrieved at any point in the architecture stack.
        /// </summary>
        /// <param name="currentTestResult">The test result object used to store results for 
        /// the current performance test.</param>
        void SetCurrentTestResult(TestResult currentTestResult);
    }
}
