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

namespace BerwickHeights.Platform.PerfTest.Model
{
    /// <summary>
    /// Abstract base class for common performance test event data.
    /// </summary>
    public abstract class PerfTestBase
    {
        private const int AnnotationMaxLen = 500;


        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        protected PerfTestBase()
        {
            StartTime = DateTime.Now;

            //
            // Set end time in case it's not set later so that the record can
            // be saved in data store with a value in accepted date range
            //
            EndTime = DateTime.Now;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PerfTestBase(DateTime startTime)
        {
            StartTime = startTime;

            //
            // Set end time in case it's not set later so that the record can
            // be saved in data store with a value in accepted date range
            //
            EndTime = startTime;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PerfTestBase(DateTime startTime, DateTime endTime, string annotation)
        {
            StartTime = startTime;
            EndTime = endTime;
            Annotation = (annotation.Length < AnnotationMaxLen) ? annotation : annotation.Substring(0, AnnotationMaxLen);
        }


        /// <summary>
        /// When event started.
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// When event finished.
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return endTime; }
            set 
            { 
                endTime = value;
                ElapsedTimeMSecs = (int) (EndTime.Subtract(StartTime).TotalMilliseconds);
            }
        }
        private DateTime endTime;

        /// <summary>
        /// How long the event took, in milliseconds. Calculated from StartTime and EndTime
        /// properties.
        /// </summary>
        public virtual int ElapsedTimeMSecs { get; private set; }

        /// <summary>
        /// Text annotation associated with event.
        /// </summary>
        public virtual string Annotation { get; set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            return ", StartTime: " + StartTime
                + ", EndTime: " + EndTime
                + ", ElapsedTimeMSecs: " + ElapsedTimeMSecs
                + ", Annotation: " + Annotation;
        }

        /// <inheritDoc/>
        public virtual bool Equals(PerfTestBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.StartTime.Equals(StartTime) 
                && other.EndTime.Equals(EndTime) 
                && Equals(other.Annotation, Annotation);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PerfTestBase)) return false;
            return Equals((PerfTestBase) obj);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = StartTime.GetHashCode();
                result = (result*397) ^ EndTime.GetHashCode();
                result = (result*397) ^ (Annotation != null ? Annotation.GetHashCode() : 0);
                return result;
            }
        }
    }
}
