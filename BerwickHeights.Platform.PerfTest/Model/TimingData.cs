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
using BerwickHeights.Platform.NHibernate.Fluent;

namespace BerwickHeights.Platform.PerfTest.Model
{
    /// <summary>
    /// Timing data for a performance test showing individual performance for sub-sections
    /// of the test.
    /// </summary>
    public class TimingData : TestResultBase, INHibernateEntity
    {
        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        public TimingData()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TimingData(DateTime startTime, DateTime endTime, string annotation, 
            string className, string methodName) 
            : base(startTime, endTime, annotation, className, methodName)
        {
        }


        /// <summary>
        /// Unique ID for this timing data.
        /// </summary>
        public virtual Guid TimingDataId { get; protected internal set; }

        /// <summary>
        /// The parent test result to which this timing data belongs.
        /// </summary>
        public virtual TestResult TestResult { get; protected internal set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            return "TimingData: "
                + "TimingDataId: " + TimingDataId
                + ", " + base.ToString();
        }

        /// <inheritDoc/>
        public virtual bool Equals(TimingData other)
        {
            return base.Equals(other);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TimingData);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
