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
using System.Text;
using BerwickHeights.Platform.NHibernate.Fluent;

namespace BerwickHeights.Platform.PerfTest.Model
{
    /// <summary>
    /// Master class for all information stored for a particular performance test run.
    /// </summary>
    public class TestSuiteResult : PerfTestBase, INHibernateEntity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TestSuiteResult(string userId) : this()
        {
            UserId = userId;
        }


        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        public TestSuiteResult()
        {
            SystemInfoList = new HashSet<SystemInfo>();
            TestResultList = new HashSet<TestResult>();
        }

        /// <summary>
        /// Unique ID for this test run.
        /// </summary>
        public virtual Guid TestSuiteResultId { get; protected internal set; }

        /// <summary>
        /// ID of user who ran the performance test.
        /// </summary>
        public virtual string UserId { get; protected internal set; }

        /// <summary>
        /// List of system information pertaining to this test run (e.g., CPU type, 
        /// amount of memory, OS type, etc.).
        /// </summary>
        public virtual ICollection<SystemInfo> SystemInfoList { get; protected internal set; }

        /// <summary>
        /// List of test results for each performance test in this test run.
        /// </summary>
        public virtual ICollection<TestResult> TestResultList { get; protected internal set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(", SystemInfoList: [");
            foreach (SystemInfo si in SystemInfoList)
            {
                sb.Append(si + "], ");
            }
            sb.Append(", TestResultList: [");
            foreach (TestResult tr in TestResultList)
            {
                sb.Append(tr + "], ");
            }
            return "TestSuiteResult: "
                + "TestSuiteResultId: " + TestSuiteResultId
                + ", UserId: " + UserId
                + ", " + base.ToString()
                + sb;
        }

        /// <inheritDoc/>
        public virtual bool Equals(TestSuiteResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) 
                && Equals(other.UserId, UserId)
                && Equals(other.SystemInfoList, SystemInfoList) 
                && Equals(other.TestResultList, TestResultList);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TestSuiteResult);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ UserId.GetHashCode();
                result = (result*397) ^ (SystemInfoList != null ? SystemInfoList.GetHashCode() : 0);
                result = (result*397) ^ (TestResultList != null ? TestResultList.GetHashCode() : 0);
                return result;
            }
        }
    }
}
