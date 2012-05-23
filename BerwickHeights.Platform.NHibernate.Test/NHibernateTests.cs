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
using System.Collections.Generic;
using System.Configuration;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.Core.Test;
using BerwickHeights.Platform.IoC;
using BerwickHeights.Platform.NHibernate.Fluent;
using BerwickHeights.Platform.NHibernate.Fluent.Conventions;
using BerwickHeights.Platform.NHibernate.Interceptors;
using BerwickHeights.Platform.PerfTest.DAL.NHibernate;
using FluentNHibernate.Automapping;
using NHibernate;
using ServiceStack.Redis;
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

            IEnumerable<string> ignoredTypes = new string[]
            {
                "BerwickHeights.Platform.Core.CurrentUser.CurrentUserSvc", 
                "BerwickHeights.Platform.IoC.Castle.Logger",
                "BerwickHeights.Platform.Config.Redis.ConfigurationSvc",
                "BerwickHeights.Platform.NHibernate.Interceptors.AuditInterceptor"
            };
            container.RegisterInterceptors(
                new InterceptorDescriptor(typeof(TransactionInterceptor), null, new string[] { "BerwickHeights" }, ignoredTypes));

            string redisNetAddr = ConfigurationManager.AppSettings["RedisNetAddr"];
            if (string.IsNullOrEmpty(redisNetAddr)) throw new Exception("Network address for Redis server not configured (RedisNetAddr)");
            string redisDbNumStr = ConfigurationManager.AppSettings["RedisDbNumber"];
            int redisDbNum = (string.IsNullOrEmpty(redisDbNumStr)) ? 0 : int.Parse(redisDbNumStr);
            container.RegisterComponentInstance(typeof(IRedisClientsManager),
                new PooledRedisClientManager(redisDbNum, redisNetAddr), "RedisClientsManager");

            // Register components in assemblies
            container.RegisterInProcComponents(
                "BerwickHeights.Platform.Core",
                "BerwickHeights.Platform.Config.Redis");
            container.RegisterComponent(typeof(ITestDataSvc), typeof(TestDataSvc));
            container.RegisterComponent(typeof(IInterceptor), typeof(AuditInterceptor));

            AutoPersistenceModel model = AutoMap
                .AssemblyOf<TestEntity>(new AutomapConfig())
                .Conventions.AddAssembly(typeof (StringConvention).Assembly);
            PerfTestFluentConfig.AutoMap(model);

            container.SetupNHibernateIntegration(FluentConfigUtils.ConfigureSqlServer2008("TestDatabase"), model, false, false);
        }

        [Test]
        public void TestPersistEntity()
        {
            CurrentUserSvcTest.SetTestCurrentUserData();

            ITestDataSvc testDataSvc = container.Resolve<ITestDataSvc>();

            TestEntity entity = new TestEntity("domainId", "externalId", 13, "data2", "http://test.com/test/foo");
            Console.WriteLine("SystemId: " + entity.SystemId);

            testDataSvc.SaveEntity(entity);

            TestEntity entityInDb = testDataSvc.GetEntity(entity.SystemId);

            Console.WriteLine("Entity in db: " + entityInDb);

            Assert.AreEqual(entity.SystemId, entityInDb.SystemId);
            Assert.AreEqual(entity, entityInDb);

            // See if cache is used in detecting transactional attribute
            testDataSvc.GetEntity(entity.SystemId);
        }
    }
}
