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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using BerwickHeights.Platform.Config.Redis;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.IoC;
using BerwickHeights.Platform.IoC.Castle;
using BerwickHeights.Platform.Logging.Log4Net;
using NUnit.Framework;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.MethodLogging.Test
{
    [TestFixture]
    public class MethodLoggingTests
    {
        private IIoCContainerManager iocContainer;

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterLoggerFactory(new Log4NetLoggerFactory());

            iocContainer.RegisterComponent(typeof(ICurrentUserSvc), typeof(CurrentUserSvc));
            iocContainer.RegisterComponent(typeof(IConfigurationSvc), typeof(ConfigurationSvc));
            iocContainer.RegisterComponent(typeof(ITestComponent), typeof(TestComponent));

            string redisNetAddr = ConfigurationManager.AppSettings["RedisNetAddr"];
            if (string.IsNullOrEmpty(redisNetAddr)) throw new Exception("Network address for Redis server not configured (RedisNetAddr)");
            iocContainer.RegisterComponentInstance(typeof(IRedisClientsManager), new PooledRedisClientManager(redisNetAddr), "RedisClientsManager");
            using (IRedisClient redis = iocContainer.Resolve<IRedisClientsManager>().GetClient())
            {
                redis.SetValue("BHS:ConfigKey:" + ConfigurationSvcBase.TraceMethodsConfigKey, "true");
            }

            MethodLoggingInterceptorBase.ConfigData loggerCfg = new MethodLoggingInterceptorBase.ConfigData(true, true, new string[] { "pass" });
            IDictionary config = new Hashtable { { MethodLoggingInterceptorBase.ConfigPropertyName, loggerCfg } };
            IEnumerable<string> namespacePrefixes = new string[] { "BerwickHeights.Platform" };
            IEnumerable<string> ignoredTypes = new string[]
            {
                "BerwickHeights.Platform.Core.CurrentUser.CurrentUserSvc", 
                "BerwickHeights.Platform.IoC.Castle.Logger",
                "BerwickHeights.Platform.Config.Redis.ConfigurationSvc"
            };
            iocContainer.RegisterInterceptors(new InterceptorDescriptor(typeof (MethodLoggingInterceptor), config, namespacePrefixes, ignoredTypes));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            using (IRedisClient redis = iocContainer.Resolve<IRedisClientsManager>().GetClient())
            {
                redis.SetValue(ConfigurationSvcBase.TraceMethodsConfigKey, "false");
            }
        }

        [Test]
        public void TestMethodLogging()
        {
            ITestComponent test = iocContainer.Resolve<ITestComponent>();
            test.Foo("arg1", 1.234f, new ArgType() { IntData = 11, StringData = "stringData"}, "password");
            try
            {
                test.FooExcept("arg1", 1.234f, new ArgType() { IntData = 11, StringData = "stringData" }, "password");
            }
            catch (Exception)
            {
            }
        }
    }
}
