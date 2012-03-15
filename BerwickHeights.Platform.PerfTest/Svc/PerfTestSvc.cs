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
using System.Transactions;
using BerwickHeights.Platform.Core.Utils;
using BerwickHeights.Platform.PerfTest.DAL;
using BerwickHeights.Platform.PerfTest.Model;
using Castle.Services.Transaction;

namespace BerwickHeights.Platform.PerfTest.Svc
{
    /// <inheritDoc/>
    public class PerfTestSvc : IPerfTestSvc
    {
        #region Private Fields

        private readonly IPerfTestDAL perfTestDAL;

        //
        // NB: By adding the ThreadStatic attribute to these private static fields, we keep the 
        // current user's performance test results in the thread's local storage so that it can be 
        // retrieved at any point in the architecture stack.
        //
        [ThreadStatic]
        static TestResult currentTestResult;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PerfTestSvc(IPerfTestDAL perfTestDAL)
        {
            this.perfTestDAL = perfTestDAL;
        }

        #endregion

        #region Implementation of IPerfTestSvc

        /// <inheritDoc/>
        [Transaction(TransactionScopeOption.Required)]
        public void SaveTestResults(TestSuiteResult testSuiteResult)
        {
            if (testSuiteResult == null)
            {
                throw new ArgumentException("The parameter 'testSuiteResult' cannot be null");
            }

            //
            // Turn off current performance test on this thread so we don't log the 
            // statistics for this save -- the added TimingData records will cause
            // problems in the save to the database since the IDs won't be set
            //
            TestResult tr = GetCurrentTestResult();
            SetCurrentTestResult(null);

            // Save test results
            perfTestDAL.SaveTestResults(testSuiteResult);

            // Resume any performance testing
            SetCurrentTestResult(tr);
        }

        /// <inheritDoc/>
        [Transaction(TransactionScopeOption.Required)]
        public TestSuiteResult GetTestResults(string testSuiteResultId)
        {
            ParamChecker.CheckParam(testSuiteResultId, "testSuiteResultId");

            return perfTestDAL.GetTestResults(testSuiteResultId);
        }

        /// <inheritDoc/>
        [Transaction(TransactionScopeOption.Required)]
        public IEnumerable<TestSuiteResult> GetTestResults(DateTime startTime, DateTime endTime)
        {
            return perfTestDAL.GetTestResults(startTime, endTime);
        }

        /// <inheritDoc/>
        public TestResult GetCurrentTestResult()
        {
            return currentTestResult;
        }

        /// <inheritDoc/>
        public void SetCurrentTestResult(TestResult curTestResult)
        {
            currentTestResult = curTestResult;
        }

        #endregion
    }
}
