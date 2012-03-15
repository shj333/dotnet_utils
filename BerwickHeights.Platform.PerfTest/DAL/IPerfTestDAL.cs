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
using BerwickHeights.Platform.PerfTest.Model;

namespace BerwickHeights.Platform.PerfTest.DAL
{
    /// <summary>
    /// Data access layer for storing and retrieving performance monitoring results in data store.
    /// </summary>
    public interface IPerfTestDAL
    {
        /// <summary>
        /// Save given performance test results in data store.
        /// </summary>
        void SaveTestResults(TestSuiteResult testSuiteResult);

        /// <summary>
        /// Retrieve given performance test results from data store.
        /// </summary>
        TestSuiteResult GetTestResults(string testSuiteResultId);

        /// <summary>
        /// Find performance test results for tests that started between the two given dates.
        /// </summary>
        IEnumerable<TestSuiteResult> GetTestResults(DateTime startTime, DateTime endTime);
    }
}
