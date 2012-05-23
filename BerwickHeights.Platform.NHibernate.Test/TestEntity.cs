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

using BerwickHeights.Platform.Core.Model;
using BerwickHeights.Platform.NHibernate.Fluent;

namespace BerwickHeights.Platform.NHibernate.Test
{
    public class TestEntity : IdBase, INHibernateEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TestEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TestEntity(string domainId, string externalId, int data1, string data2, string someUrl) 
            : base(domainId, externalId)
        {
            Data1 = data1;
            Data2 = data2;
            SomeUrl = someUrl;
        }

        public virtual int Data1 { get; protected internal set; }

        public virtual string Data2 { get; protected internal set; }

        public virtual string SomeUrl { get; protected internal set; }

        public override string ToString()
        {
            return "TestEntity: "
                + ", Data1: " + Data1
                + ", Data2: " + Data2
                + ", SomeUrl: " + SomeUrl
                + base.ToString();
        }

        public virtual bool Equals(TestEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.Data1 == Data1 && Equals(other.Data2, Data2) && Equals(other.SomeUrl, SomeUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TestEntity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ Data1;
                result = (result*397) ^ (Data2 != null ? Data2.GetHashCode() : 0);
                result = (result*397) ^ (SomeUrl != null ? SomeUrl.GetHashCode() : 0);
                return result;
            }
        }
    }
}
