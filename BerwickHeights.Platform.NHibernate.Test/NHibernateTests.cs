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
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.IoC;
using BerwickHeights.Platform.NHibernate.Fluent;
using BerwickHeights.Platform.NHibernate.Fluent.Conventions;
using BerwickHeights.Platform.NHibernate.Interceptors;
using BerwickHeights.Platform.PerfTest.DAL.NHibernate;
using FluentNHibernate.Automapping;
using NHCfg = NHibernate.Cfg;
using NUnit.Framework;
using ILoggerFactory = BerwickHeights.Platform.Logging;
using Log4NetLoggerFactory = BerwickHeights.Platform.Logging.Log4Net.Log4NetLoggerFactory;

namespace BerwickHeights.Platform.NHibernate.Test
{
    [TestFixture]
    public class NHibernateTests
    {
        private IIoCContainerManager container;

        [TestFixtureSetUp]
        public void Init()
        {
            container = IoCContainerManagerFactory.GetIoCContainerManager();
            container.RegisterLoggerFactory(new Log4NetLoggerFactory());

            InterceptorDescriptor descriptor = new InterceptorDescriptor(
                typeof(TransactionInterceptor), 
                null, 
                new string[] {"BerwickHeights"}, 
                new string[0]);
            container.RegisterInterceptors(descriptor);

            AutoPersistenceModel model = AutoMap
                .AssemblyOf<TestEntity>(new AutomapConfig())
                .Conventions.AddAssembly(typeof(StringConvention).Assembly);
            PerfTestFluentConfig.AutoMap(model);

            container.SetupNHibernateIntegration(FluentConfigUtils.ConfigureSqlServer2008("TestDatabase"), model, false, false);

            container.RegisterComponent(typeof(ITestDataSvc), typeof(TestDataSvc));
        }

        [Test]
        public void TestPersistEntity()
        {
            ITestDataSvc testDataSvc = container.Resolve<ITestDataSvc>();

            TestEntity entity = new TestEntity(13, "data2", "http://test.com/test/foo");
            Console.WriteLine("TestEntityId: " + entity.TestEntityId);

            testDataSvc.SaveEntity(entity);

            TestEntity entityInDb = testDataSvc.GetEntity(entity.TestEntityId);

            Console.WriteLine("Entity in db: " + entityInDb);

            Assert.AreEqual(entity.TestEntityId, entityInDb.TestEntityId);
            Assert.AreEqual(entity, entityInDb);

            // See if cache is used in detecting transactional attribute
            testDataSvc.GetEntity(entity.TestEntityId);
        }
    }
}
