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
        public TestEntity(int data1, string data2)
        {
            TestEntityId = Guid.NewGuid().ToString();
            Data1 = data1;
            Data2 = data2;
        }

        public virtual string TestEntityId { get; protected internal set; }

        public virtual int Data1 { get; protected internal set; }

        public virtual string Data2 { get; protected internal set; }
    }
}
