using System;

namespace BerwickHeights.Platform.NHibernate.Test
{
    public class TestEntity
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
        public TestEntity(int data1, string data2, string someUrl)
        {
            TestEntityId = Guid.NewGuid().ToString();
            Data1 = data1;
            Data2 = data2;
            SomeUrl = someUrl;
        }

        public virtual string TestEntityId { get; protected internal set; }

        public virtual int Data1 { get; protected internal set; }

        public virtual string Data2 { get; protected internal set; }

        public virtual string SomeUrl { get; protected internal set; }

        public override string ToString()
        {
            return "TestEntity: "
                + "TestEntityId: " + TestEntityId
                + ", Data1: " + Data1
                + ", Data2: " + Data2
                + ", SomeUrl: " + SomeUrl;
        }

        public virtual bool Equals(TestEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Data1 == Data1 && Equals(other.Data2, Data2) && Equals(other.SomeUrl, SomeUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TestEntity)) return false;
            return Equals((TestEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Data1;
                result = (result*397) ^ (Data2 != null ? Data2.GetHashCode() : 0);
                result = (result*397) ^ (SomeUrl != null ? SomeUrl.GetHashCode() : 0);
                return result;
            }
        }
    }
}
