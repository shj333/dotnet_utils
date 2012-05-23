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
using Castle.Services.Transaction;
using NHibernate;

namespace BerwickHeights.Platform.NHibernate.Test
{
    public interface ITestDataSvc
    {
        void SaveEntity(TestEntity testEntity);

        TestEntity GetEntity(Guid testEntityId);
    }

    public class TestDataSvc : ITestDataSvc
    {
        private readonly ISessionFactory sessionFactory;

        public TestDataSvc(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        [Transaction]
        public void SaveEntity(TestEntity testEntity)
        {
            ISession session = sessionFactory.GetCurrentSession();
            session.Save(testEntity);
        }

        [Transaction]
        public TestEntity GetEntity(Guid testEntityId)
        {
            ISession session = sessionFactory.GetCurrentSession();
            return session.Get<TestEntity>(testEntityId);
        }
    }
}
