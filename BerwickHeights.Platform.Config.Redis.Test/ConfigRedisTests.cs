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
using BerwickHeights.Platform.IoC;
using BerwickHeights.Platform.Logging.Log4Net;
using NUnit.Framework;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.Config.Redis.Test
{
    [TestFixture]
    public class ConfigRedisTests
    {
        private IIoCContainerManager iocContainer;
        private const string testKeyPrefix = "BHS:ConfigKey:";
        private const string testBoolCfgTrueKey = "TestBoolCfgTrue";
        private const string testBoolCfgFalseKey = "TestBoolFalseTrue";
        private const string testIntCfgKey = "TestIntCfg";
        private const int testInt = 753;
        private const string testStrCfgKey = "TestStrCfg";
        private const string testStr = "Test string 135";
        private const string testDfltStr = "Test string default";
        private const string testStrArrayCfgKey = "TestStrArrayCfg";
        private readonly string[] testStrArray = new string[] { "aaa", "bbb", "ccc"};
        private const string testPasswordCfgKey = "TestPassword";

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterLoggerFactory(new Log4NetLoggerFactory());

            string redisNetAddr = ConfigurationManager.AppSettings["RedisNetAddr"];
            if (string.IsNullOrEmpty(redisNetAddr)) throw new Exception("Network address for Redis server not configured (RedisNetAddr)");
            iocContainer.RegisterComponentInstance(typeof(IRedisClientsManager), new PooledRedisClientManager(redisNetAddr), "RedisClientsManager");
            iocContainer.RegisterComponent(typeof(IConfigurationSvc), typeof(ConfigurationSvc));

            using (IRedisClient redis = iocContainer.Resolve<IRedisClientsManager>().GetClient())
            {
                redis.SetEntry(testKeyPrefix + testBoolCfgTrueKey, "true");
                redis.SetEntry(testKeyPrefix + testBoolCfgFalseKey, "false");
                redis.SetEntry(testKeyPrefix + testIntCfgKey, testInt + "");
                redis.SetEntry(testKeyPrefix + testStrCfgKey, testStr);
                string val = testStrArray.Aggregate("", (current, str) => current + (str + ","));
                val = val.Substring(0, val.Length - 1);
                redis.SetEntry(testKeyPrefix + testStrArrayCfgKey, val);
            }
        }


        [Test]
        public void TestConfigViaRedis()
        {
            // Boolean config
            IConfigurationSvc configurationSvc = iocContainer.Resolve<IConfigurationSvc>();
            Assert.IsTrue(configurationSvc.GetBooleanConfig(testBoolCfgTrueKey, false));
            Assert.IsFalse(configurationSvc.GetBooleanConfig(testBoolCfgFalseKey, true));
            Assert.IsTrue(configurationSvc.GetBooleanConfig(testBoolCfgTrueKey + Guid.NewGuid(), true));

            // Int config
            Assert.AreEqual(testInt, configurationSvc.GetIntConfig(testIntCfgKey));
            bool exceptThrown = false;
            try
            {
                Assert.AreEqual(testInt, configurationSvc.GetIntConfig(testIntCfgKey + Guid.NewGuid(), true));
            }
            catch (ConfigurationErrorsException)
            {
                exceptThrown = true;
            }
            Assert.IsTrue(exceptThrown);
            Assert.AreEqual(13, configurationSvc.GetIntConfig(testIntCfgKey + Guid.NewGuid(), false, 13));

            // String config
            Assert.AreEqual(testStr, configurationSvc.GetStringConfig(testStrCfgKey, true));
            exceptThrown = false;
            try
            {
                Assert.AreEqual(testStr, configurationSvc.GetStringConfig(testStrCfgKey + Guid.NewGuid(), true));
            }
            catch (ConfigurationErrorsException)
            {
                exceptThrown = true;
            }
            Assert.IsTrue(exceptThrown);
            Assert.AreEqual(testDfltStr, configurationSvc.GetStringConfig(testStrCfgKey + Guid.NewGuid(), false, testDfltStr));

            // String array config
            string[] val = configurationSvc.GetStringArrayConfig(testStrArrayCfgKey);
            Assert.AreEqual(testStrArray.Length, val.Length);
            for (int idx = 0; idx < testStrArray.Length; idx++) Assert.AreEqual(testStrArray[idx], val[idx]);

            string password = configurationSvc.GetStringConfig(testPasswordCfgKey);
            Assert.AreEqual(password, password);
        }
    }
}
