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

namespace BerwickHeights.Platform.PerfTest.Test
{
    /// <summary>
    /// Interface for application-specific functionality that is run by each application-defined type that
    /// runs performance tests.
    /// </summary>
    public interface IPerfTestApp
    {
        /// <summary>
        /// This is run before all the methods marked with the "PerfTest" attribute are run. Allows app-specific
        /// logic to set up the performance tests.
        /// </summary>
        void Setup();

        /// <summary>
        /// This is run after all the methods marked with the "PerfTest" attribute are run. Allows app-specific logic 
        /// to add perf test statistics to the annotation for the performance test results persisted in the data store.
        /// </summary>
        string GetStats();

        /// <summary>
        /// This is run after all the methods marked with the "PerfTest" attribute are run. Allows app-specific
        /// logic to clean up after the performance tests have run.
        /// </summary>
        void TearDown();
    }
}
