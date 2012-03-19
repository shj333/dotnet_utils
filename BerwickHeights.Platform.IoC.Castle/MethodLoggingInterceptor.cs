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
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.Logging;
using BerwickHeights.Platform.MethodLogging;
using BerwickHeights.Platform.PerfTest.Svc;
using Castle.DynamicProxy;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Castle Windsor interceptor that logs method calls and any exceptions that occur during method call.
    /// </summary>
    public class MethodLoggingInterceptor : MethodLoggingInterceptorBase, IInterceptor
    {
        #region Constructors

        /// <summary>
        /// Constructor used when running performance tests
        /// </summary>
        public MethodLoggingInterceptor(IPerfTestSvc perfTestSvc, 
            ICurrentUserSvc currentUserSvc, 
            IConfigurationSvc configurationSvc,
            ILogger logger)
            : base(perfTestSvc, currentUserSvc, configurationSvc, logger)
        {
        }

        /// <summary>
        /// Production constructor (no PerfTest component)
        /// </summary>
        public MethodLoggingInterceptor(ICurrentUserSvc currentUserSvc,
            IConfigurationSvc configurationSvc,
            ILogger logger)
            : this(null, currentUserSvc, configurationSvc, logger)
        {
        }
        
        #endregion

        #region Implementation of IInterceptor

        /// <inheritDoc/>
        public void Intercept(IInvocation invocation)
        {
            InterceptMethodCall(invocation, invocation.TargetType.Name, invocation.Method.Name, 
                invocation.Method.GetParameters(), invocation.Arguments, invocation.Method.ReturnType);
        }

        #endregion

        #region Overrides of MethodLoggingInterceptorBase

        /// <inheritDoc/>
        protected override object ProceedWithMethodCall(object data)
        {
            IInvocation invocation = data as IInvocation;
            if (invocation == null) throw new Exception("Invocation data is of wrong type: " + data);
            invocation.Proceed();
            return invocation.ReturnValue;
        }

        #endregion
    }
}
