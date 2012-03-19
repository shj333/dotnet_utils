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
using System.Linq;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.IoC;
using NUnit.Framework;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.Config.Redis.Test
{
    [TestFixture]
    public class TestConfigRedis
    {
        private IIoCContainerManager iocContainer;
        private const string TestKeyPrefix = "BHS:ConfigKey:";
        private const string TestBoolCfgTrueKey = "TestBoolCfgTrue";
        private const string TestBoolCfgFalseKey = "TestBoolFalseTrue";
        private const string TestIntCfgKey = "TestIntCfg";
        private const int TestInt = 753;
        private const string TestStrCfgKey = "TestStrCfg";
        private const string TestStr = "Test string 135";
        private const string TestDfltStr = "Test string default";
        private const string TestStrArrayCfgKey = "TestStrArrayCfg";
        private readonly string[] testStrArray = new string[] { "aaa", "bbb", "ccc"};

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();

            string redisNetAddr = ConfigurationManager.AppSettings["RedisNetAddr"];
            if (string.IsNullOrEmpty(redisNetAddr)) throw new Exception("Network address for Redis server not configured (RedisNetAddr)");
            iocContainer.RegisterComponentInstance(typeof(IRedisClientsManager), new PooledRedisClientManager(redisNetAddr), "RedisClientsManager");
            iocContainer.RegisterComponent(typeof(IConfigurationSvc), typeof(ConfigurationSvc));

            using (IRedisClient redis = iocContainer.Resolve<IRedisClientsManager>().GetClient())
            {
                redis.SetEntry(TestKeyPrefix + TestBoolCfgTrueKey, "true");
                redis.SetEntry(TestKeyPrefix + TestBoolCfgFalseKey, "false");
                redis.SetEntry(TestKeyPrefix + TestIntCfgKey, TestInt + "");
                redis.SetEntry(TestKeyPrefix + TestStrCfgKey, TestStr);
                string val = testStrArray.Aggregate("", (current, str) => current + (str + ","));
                val = val.Substring(0, val.Length - 1);
                redis.SetEntry(TestKeyPrefix + TestStrArrayCfgKey, val);
            }
        }


        [Test]
        public void TestConfigViaRedis()
        {
            // Boolean config
            IConfigurationSvc configurationSvc = iocContainer.Resolve<IConfigurationSvc>();
            Assert.IsTrue(configurationSvc.GetBooleanConfig(TestBoolCfgTrueKey, false));
            Assert.IsFalse(configurationSvc.GetBooleanConfig(TestBoolCfgFalseKey, true));
            Assert.IsTrue(configurationSvc.GetBooleanConfig(TestBoolCfgTrueKey + Guid.NewGuid(), true));

            // Int config
            Assert.AreEqual(TestInt, configurationSvc.GetIntConfig(TestIntCfgKey));
            bool exceptThrown = false;
            try
            {
                Assert.AreEqual(TestInt, configurationSvc.GetIntConfig(TestIntCfgKey + Guid.NewGuid(), true));
            }
            catch (ConfigurationErrorsException)
            {
                exceptThrown = true;
            }
            Assert.IsTrue(exceptThrown);
            Assert.AreEqual(13, configurationSvc.GetIntConfig(TestIntCfgKey + Guid.NewGuid(), false, 13));

            // String config
            Assert.AreEqual(TestStr, configurationSvc.GetStringConfig(TestStrCfgKey, true));
            exceptThrown = false;
            try
            {
                Assert.AreEqual(TestStr, configurationSvc.GetStringConfig(TestStrCfgKey + Guid.NewGuid(), true));
            }
            catch (ConfigurationErrorsException)
            {
                exceptThrown = true;
            }
            Assert.IsTrue(exceptThrown);
            Assert.AreEqual(TestDfltStr, configurationSvc.GetStringConfig(TestStrCfgKey + Guid.NewGuid(), false, TestDfltStr));

            // String array config
            string[] val = configurationSvc.GetStringArrayConfig(TestStrArrayCfgKey);
            Assert.AreEqual(testStrArray.Length, val.Length);
            for (int idx = 0; idx < testStrArray.Length; idx++) Assert.AreEqual(testStrArray[idx], val[idx]);
        }
    }
}
