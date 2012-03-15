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

namespace BerwickHeights.Platform.PerfTest.Model
{
    /// <summary>
    /// One piece of system information associated with a performance test run. Examples
    /// include CPU type, amount of memory, OS type, etc.
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        public SystemInfo()
        {
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public SystemInfo(string component, string name, string value)
        {
            Component = component;
            Name = name;
            Value = value;
        }


        /// <summary>
        /// Unique identifier for this piece of system information.
        /// </summary>
        public virtual string SystemInfoId { get; private set; }

        /// <summary>
        /// The type of component that this piece of system information applies to
        /// (e.g., cpu, memory).
        /// </summary>
        public virtual string Component { get; private set; }

        /// <summary>
        /// The name of this piece of system information.
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// The value of this piece of system infomation.
        /// </summary>
        public virtual string Value { get; private set; }

        /// <summary>
        /// The parent test run to which this system information belongs.
        /// </summary>
        public virtual TestSuiteResult TestSuiteResult { get; protected internal set; }


        /// <inheritDoc/>
        public override string ToString()
        {
            return "SystemInfo: "
                + "SystemInfoId: " + SystemInfoId
                + ", Component: " + Component
                + ", Name: " + Name
                + ", Value: " + Value;
        }

        /// <inheritDoc/>
        public virtual bool Equals(SystemInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Component, Component) && Equals(other.Name, Name) && Equals(other.Value, Value);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SystemInfo)) return false;
            return Equals((SystemInfo) obj);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Component != null ? Component.GetHashCode() : 0);
                result = (result*397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result*397) ^ (Value != null ? Value.GetHashCode() : 0);
                return result;
            }
        }
    }
}
