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
    /// Abstract base class for holding data pertaining to a test.
    /// </summary>
    public abstract class TestResultBase : PerfTestBase
    {
        private const int ClassNameMaxLen = 200;
        private const int MethodNameMaxLen = 200;
        
        
        /// <summary>
        /// Parameterless constructor needed for NHibernate.
        /// </summary>
        protected TestResultBase()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TestResultBase(string className, string methodName)
        {
            InitInstance(className, methodName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TestResultBase(DateTime startTime, string className, string methodName)
            : base(startTime)
        {
            InitInstance(className, methodName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TestResultBase(DateTime startTime, DateTime endTime, string annotation, 
            string className, string methodName)
            : base(startTime, endTime, annotation)
        {
            InitInstance(className, methodName);
        }

        private void InitInstance(string className, string methodName)
        {
            ClassName = (className.Length < ClassNameMaxLen) ? className : className.Substring(0, ClassNameMaxLen);
            MethodName = (methodName.Length < MethodNameMaxLen) ? methodName : methodName.Substring(0, MethodNameMaxLen);
        }


        /// <summary>
        /// The class that generated this test data.
        /// </summary>
        public virtual string ClassName { get; private set; }

        /// <summary>
        /// The method that generated this test data.
        /// </summary>
        public virtual string MethodName { get; private set; }

        /// <inheritDoc/>
        public override string ToString()
        {
            return ", ClassName: " + ClassName
                + ", MethodName: " + MethodName
                + base.ToString();
        }

        /// <inheritDoc/>
        public virtual bool Equals(TestResultBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) 
                && Equals(other.ClassName, ClassName) 
                && Equals(other.MethodName, MethodName);
        }

        /// <inheritDoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TestResultBase);
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ (ClassName != null ? ClassName.GetHashCode() : 0);
                result = (result*397) ^ (MethodName != null ? MethodName.GetHashCode() : 0);
                return result;
            }
        }
    }
}
