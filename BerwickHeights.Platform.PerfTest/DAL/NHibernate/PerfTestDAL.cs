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
using System.Linq;
using BerwickHeights.Platform.NHibernate.DAL;
using BerwickHeights.Platform.PerfTest.Model;
using NHibernate;

namespace BerwickHeights.Platform.PerfTest.DAL.NHibernate
{
    /// <inheritDoc/>
    public class PerfTestDAL : PersistenceDALBase, IPerfTestDAL
    {
        /// <inheritDoc/>
        public PerfTestDAL(ISessionFactory sessionFactory, Core.Logging.ILoggerFactory loggerFactory)
            : base(sessionFactory, loggerFactory)
        {
        }

        /// <inheritDoc/>
        public virtual void SaveTestResults(TestSuiteResult testSuiteResult)
        {
            // Set parent objects in children if not already set
            foreach (TestResult testResult in testSuiteResult.TestResultList)
            {
                if (testResult.TestSuiteResult == null) testResult.TestSuiteResult = testSuiteResult;
                foreach (TimingData timingData in testResult.TimingDataList.Where(
                    timingData => timingData.TestResult == null))
                {
                    timingData.TestResult = testResult;
                }
            }
            foreach (SystemInfo systemInfo in testSuiteResult.SystemInfoList.Where(
                systemInfo => systemInfo.TestSuiteResult == null))
            {
                systemInfo.TestSuiteResult = testSuiteResult;
            }

            // Cascades save to persist/update children performance test data as well
            ISession session = sessionFactory.GetCurrentSession();
            session.Save(testSuiteResult);
        }

        /// <inheritDoc/>
        public virtual TestSuiteResult GetTestResults(string testSuiteResultId)
        {
            ISession session = sessionFactory.GetCurrentSession();
            TestSuiteResult testSuiteResult = session.Get<TestSuiteResult>(testSuiteResultId);
            if (testSuiteResult != null) NHibernateUtil.Initialize(testSuiteResult);
            return testSuiteResult;
        }

        /// <inheritDoc/>
        public virtual IEnumerable<TestSuiteResult> GetTestResults(DateTime startTime, DateTime endTime)
        {
            ISession session = sessionFactory.GetCurrentSession();
            const string hql = "from TestSuiteResult tr "
                + "where tr.StartTime >= :startTime "
                + "and tr.StartTime <= :endTime ";
            return session.CreateQuery(hql)
                .SetDateTime("startTime", startTime)
                .SetDateTime("endTime", endTime)
                .List<TestSuiteResult>();
        }
    }
}
