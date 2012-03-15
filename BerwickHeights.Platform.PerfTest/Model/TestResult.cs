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
using System.Text;

namespace BerwickHeights.Platform.PerfTest.Model
{
    /// <summary>
    /// Performance data associated with a performance test (e.g., class and method that generated 
    /// this data, when it took place, how long it took, whether or not it was successful etc).
    /// </summary>
    public class TestResult : TestResultBase
    {
        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        public TestResult()
        {
            InitInstance();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestResult(string className, string methodName) 
            : base(className, methodName)
        {
            InitInstance();
        }

        private void InitInstance()
        {
            TimingDataList = new HashSet<TimingData>();
            IsSuccess = true;
        }


        /// <summary>
        /// Unique ID for this test data.
        /// </summary>
        public virtual string TestResultId { get; private set; }

        /// <summary>
        /// Whether or not this test was successful.
        /// </summary>
        public virtual bool IsSuccess { get; set; }

        /// <summary>
        /// A list of timing data for this test showing individual performance for sub-sections
        /// of this test.
        /// </summary>
        public virtual ICollection<TimingData> TimingDataList { get; private set; }

        /// <summary>
        /// The parent test run to which this data belongs.
        /// </summary>
        public virtual TestSuiteResult TestSuiteResult { get; protected internal set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(", TimingDataList: [");
            foreach (TimingData td in TimingDataList)
            {
                sb.Append(td + "], ");
            }
            return "TestResult: "
                + "TestResultId: " + TestResultId
                + ", IsSuccess: " + IsSuccess
                + base.ToString()
                + sb;
        }

        /// <inheritDoc/>
        public virtual bool Equals(TestResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.IsSuccess.Equals(IsSuccess);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TestResult);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ IsSuccess.GetHashCode();
            }
        }
    }
}
