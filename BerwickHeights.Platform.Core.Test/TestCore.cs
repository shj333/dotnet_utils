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
using BerwickHeights.Platform.IoC;
using NUnit.Framework;

namespace BerwickHeights.Platform.Core.Test
{
    [TestFixture]
    public class TestCore
    {
        private IIoCContainerManager iocContainer;
        private ICurrentUserSvc currentUserSvc;

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterComponent(typeof(ICurrentUserSvc), typeof(CurrentUserSvc));
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
    }
}
