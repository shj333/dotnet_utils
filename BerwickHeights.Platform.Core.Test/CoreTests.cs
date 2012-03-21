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
using System.Threading;
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.Core.Xml;
using BerwickHeights.Platform.IoC;
using NUnit.Framework;

namespace BerwickHeights.Platform.Core.Test
{
    [TestFixture]
    public class CoreTests
    {
        private IIoCContainerManager iocContainer;
        private ICurrentUserSvc currentUserSvc;
        private IXmlProcessorSvc xmlProcessorSvc;

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterComponent(typeof(ICurrentUserSvc), typeof(CurrentUserSvc));
            iocContainer.RegisterComponent(typeof(IXmlProcessorSvc), typeof(XmlProcessorSvc));
        }
   

        [Test]
        public void TestCurrentUser()
        {
            currentUserSvc = iocContainer.Resolve<ICurrentUserSvc>();
            List<Thread> testThreads = new List<Thread>();
            for (int idx = 0; idx < 10; idx++) testThreads.Add(new Thread(TestCurrentUserThrd));
            foreach (Thread ts in testThreads) ts.Start();
            foreach (Thread ts in testThreads) ts.Join(5000);
        }


        private void TestCurrentUserThrd()
        {
            CurrentUserData currentUserData = currentUserSvc.GetCurrentUserData();
            Assert.IsFalse(currentUserData.IsInitialized);

            string userId = Guid.NewGuid().ToString();
            string userName = Guid.NewGuid().ToString();
            string sessionId = Guid.NewGuid().ToString();
            IDictionary<string, string> attributes= new Dictionary<string, string>()
            {
                { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
            };

            currentUserSvc.SetCurrentUserData(new CurrentUserData(userId, userName, sessionId, attributes));
            Thread.Sleep(2000);
            currentUserData = currentUserSvc.GetCurrentUserData();
            Assert.IsTrue(currentUserData.IsInitialized);
            Assert.AreEqual(userId, currentUserData.UserId);
            Assert.AreEqual(userName, currentUserData.UserName);
            Assert.AreEqual(sessionId, currentUserData.SessionId);
            foreach (string key in attributes.Keys) Assert.IsTrue(currentUserData.HasAttribute(key));
            foreach (string key in attributes.Keys) Assert.AreEqual(attributes[key], currentUserData.GetAttribute(key));

            currentUserSvc.ResetCurrentUserData();
            currentUserData = currentUserSvc.GetCurrentUserData();
            Assert.IsFalse(currentUserData.IsInitialized);
        }

        [Test]
        public void TestXml()
        {
            xmlProcessorSvc = iocContainer.Resolve<IXmlProcessorSvc>();
            xmlProcessorSvc.LoadXsdSchema(new XsdDescriptor("BerwickHeights.Platform.Core.Test.dll", "BerwickHeights.Platform.Core.Test.Test.xsd"));
            xmlProcessorSvc.LoadXsdSchema(new XsdDescriptor("BerwickHeights.Platform.Core.Test.dll", "BerwickHeights.Platform.Core.Test.XSD.Other.Test2.xsd"));

            TestDataType data = new TestDataType() { Data1 = "data1", Data2 = "data2", Data3 = new TestDataType2() { Data21="data21", Data22 = "data22"} };
            string xml = xmlProcessorSvc.Serialize(data);
            ValidationResult result = xmlProcessorSvc.Validate(xml);
            Assert.IsTrue(result.IsValid);
            TestDataType data2 = xmlProcessorSvc.Deserialize<TestDataType>(xml);
            Assert.AreEqual(data.Data1, data2.Data1);
            Assert.AreEqual(data.Data2, data2.Data2);
            Assert.AreEqual(data.Data3.Data21, data2.Data3.Data21);
            Assert.AreEqual(data.Data3.Data22, data2.Data3.Data22);
            Assert.AreEqual(data.Data4, data2.Data4);

            result = xmlProcessorSvc.Validate("<?xml version=\"1.0\"?><TestData xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Data4=\"data3\" xmlns=\"http://xsd.berwickheights.com/Test\"><Data5>data1</Data5><Data6>data2</Data6></TestData>");
            Assert.IsFalse(result.IsValid);
        }
    }
}
