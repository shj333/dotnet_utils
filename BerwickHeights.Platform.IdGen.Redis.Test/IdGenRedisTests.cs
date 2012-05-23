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
using System.Configuration;
using BerwickHeights.Platform.Config.Redis;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.IdGen;
using BerwickHeights.Platform.IoC;
using BerwickHeights.Platform.Logging.Log4Net;
using NUnit.Framework;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.IdGen.Redis.Test
{
    [TestFixture]
    public class IdGenRedisTests
    {
        private IIoCContainerManager iocContainer;
        private IIdGeneratorSvc generatorSvc;

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterLoggerFactory(new Log4NetLoggerFactory());

            string redisNetAddr = ConfigurationManager.AppSettings["RedisNetAddr"];
            if (string.IsNullOrEmpty(redisNetAddr)) throw new Exception("Network address for Redis server not configured (RedisNetAddr)");
            iocContainer.RegisterComponentInstance(typeof(IRedisClientsManager), new PooledRedisClientManager(redisNetAddr), "RedisClientsManager");
            iocContainer.RegisterComponent(typeof(IConfigurationSvc), typeof(ConfigurationSvc));
            iocContainer.RegisterComponent(typeof(IIdGeneratorSvc), typeof(IdGeneratorSvc));

            generatorSvc = iocContainer.Resolve<IIdGeneratorSvc>();
        }

        [Test]
        public void TestIdGen()
        {
            string appId1 = Guid.NewGuid().ToString();
            string appId2 = Guid.NewGuid().ToString();

            const int objTypeId1 = 1;
            const int objTypeId2 = 2;

            long id11 = generatorSvc.GetNextId(appId1, objTypeId1);
            long id12 = generatorSvc.GetNextId(appId1, objTypeId2);
            long id21 = generatorSvc.GetNextId(appId2, objTypeId1);
            long id22 = generatorSvc.GetNextId(appId2, objTypeId2);

            for (int cnt = 1; cnt < 10; cnt++)
            {
                id11 = TestIdGen(appId1, objTypeId1, id11);
                id12 = TestIdGen(appId1, objTypeId2, id12);
                id21 = TestIdGen(appId2, objTypeId1, id21);
                id22 = TestIdGen(appId2, objTypeId2, id22);
            }
        }

        private long TestIdGen(string appId, int objTypeId, long curId)
        {
            long id = generatorSvc.GetNextId(appId, objTypeId);
            Assert.AreEqual(curId + 1, id);
            id = generatorSvc.GetNextId(appId, objTypeId);
            Assert.AreEqual(curId + 2, id);
            id = generatorSvc.GetNextId(appId, objTypeId);
            Assert.AreEqual(curId + 3, id);
            return id;
        }
    }
}
