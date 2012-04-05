﻿/*
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
using BerwickHeights.Platform.NHibernate;
using BerwickHeights.Platform.PerfTest.Model;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;

namespace BerwickHeights.Platform.PerfTest.DAL.NHibernate
{
    /// <inheritDoc/>
    public class PerfTestDAL : PersistenceDALBase, IPerfTestDAL
    {
        /// <inheritDoc/>
        public PerfTestDAL(ISessionManager sessionManager, Core.Logging.ILoggerFactory loggerFactory)
            : base(sessionManager, loggerFactory)
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
            genericDao.Save(testSuiteResult);
        }

        /// <inheritDoc/>
        public virtual TestSuiteResult GetTestResults(string testSuiteResultId)
        {
            TestSuiteResult testSuiteResult;
            using (sessionManager.OpenSession())
            {
                try
                {
                    testSuiteResult = genericDao.FindById(typeof(TestSuiteResult),
                        testSuiteResultId) as TestSuiteResult;
                    NHibernateUtil.Initialize(testSuiteResult);
                }
                catch (ObjectNotFoundException)
                {
                    testSuiteResult = null;
                }
            }

            return testSuiteResult;
        }

        /// <inheritDoc/>
        public virtual IEnumerable<TestSuiteResult> GetTestResults(DateTime startTime, DateTime endTime)
        {
            using (ISession session = sessionManager.OpenSession())
            {
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
}
