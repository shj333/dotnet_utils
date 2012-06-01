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
using System.Linq;
using System.Reflection;
using System.Text;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.Logging;
using BerwickHeights.Platform.PerfTest.Model;
using BerwickHeights.Platform.PerfTest.Svc;

namespace BerwickHeights.Platform.MethodLogging
{
    /// <summary>
    /// Abstract base class for an AOP interceptor that logs method calls and exceptions that occur during
    /// the method call.
    /// </summary>
    public abstract class MethodLoggingInterceptorBase
    {
        /// <summary>
        /// Configuration used in the methods logging interceptor.
        /// </summary>
        public class ConfigData
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            public ConfigData(bool dumpMethodParameters, bool dumpMethodReturnValue, 
                IEnumerable<string> parameterNameIgnoreList)
            {
                DumpMethodParameters = dumpMethodParameters;
                DumpMethodReturnValue = dumpMethodReturnValue;
                ParameterNameIgnoreList = parameterNameIgnoreList.Where(p => !string.IsNullOrEmpty(p));
            }
            /// <summary>
            /// Controls if method call parameters are to be dumped or not.
            /// </summary>
            public bool DumpMethodParameters { get; private set; }
            /// <summary>
            /// Controls if method call returned value is to be dumped or not.
            /// </summary>
            public bool DumpMethodReturnValue { get; private set; }
            /// <summary>
            /// Parameter names that are to be ignored when dumping parameter values (e.g., don't dump password values).
            /// </summary>
            public IEnumerable<string> ParameterNameIgnoreList { get; private set; }
        }
        
        private readonly IPerfTestSvc perfTestSvc;
        private readonly ICurrentUserSvc currentUserSvc;
        private readonly IConfigurationSvc configurationSvc;
        private readonly ILogger logger;

        /// <summary>
        /// Interceptor config property name
        /// </summary>
        public const string ConfigPropertyName = "Config";
        /// <summary>
        /// Gets or sets interceptor's configuration data.
        /// </summary>
        public ConfigData Config { get; set; }
        
        
        /// <summary>
        /// Constructor used when running performance tests
        /// </summary>
        protected MethodLoggingInterceptorBase(IPerfTestSvc perfTestSvc, 
            ICurrentUserSvc currentUserSvc,
            IConfigurationSvc configurationSvc,
            ILoggerFactory loggerFactory)
        {
            this.perfTestSvc = perfTestSvc;
            this.currentUserSvc = currentUserSvc;
            this.configurationSvc = configurationSvc;
            logger = loggerFactory.GetLogger(GetType());
        }

        /// <summary>
        /// Production constructor (no PerfTest component)
        /// </summary>
        protected MethodLoggingInterceptorBase(ICurrentUserSvc currentUserSvc, 
            IConfigurationSvc configurationSvc,
            ILoggerFactory loggerFactory)
            : this(null, currentUserSvc, configurationSvc, loggerFactory)
        {
        }

        /// <summary>
        /// Implementing type can use this to log the method call (if configured via 
        /// ConfigurationSvcBase.TraceMethodsConfigKey configuration key) and log any exceptions that are caught.
        /// The method calls the abstract method ProceedWithMethodCall() when it is time to call the intercepted 
        /// method. The data object passed into this method is returned in the call to ProceedWithMethodCall().
        /// </summary>
        /// <param name="data">Implementation-specific data (e.g., invocation data) that is returned in the call
        /// to ProceedWithMethodCall().</param>
        /// <param name="typeName">The type of the target class.</param>
        /// <param name="methodName">The name of the method being intercepted.</param>
        /// <param name="methodParameters">The parameters of the method being intercepted.</param>
        /// <param name="arguments">The argument values of the method being intercepted.</param>
        /// <param name="returnType">The return type of the method being intercepted.</param>
        protected void InterceptMethodCall(object data, string typeName, string methodName, 
            IList<ParameterInfo> methodParameters, IList<object> arguments, Type returnType)
        {
            StringBuilder sb = null;
            bool isTraceMethods = configurationSvc.GetBooleanConfig(ConfigurationSvcBase.TraceMethodsConfigKey, false);

            if (isTraceMethods)
            {
                sb = new StringBuilder("Method Call: ");
                DumpMethodCallData(typeName, methodName, methodParameters, arguments, false, sb);
            }

            // See if test data is available -- is set only if running performance test
            TestResult currentTestResult = (perfTestSvc == null) ? null : perfTestSvc.GetCurrentTestResult();
            bool isPerfTest = (currentTestResult != null);

            object returnValue;
            try
            {
                // Keep track of timing for method call for performance tests
                DateTime startTime = (isPerfTest) ? DateTime.Now : DateTime.MinValue;

                // Call method
                returnValue = ProceedWithMethodCall(data);

                // If running a performance test
                if (isPerfTest)
                {
                    // Add timing data to performance test results
                    currentTestResult.TimingDataList.Add(new TimingData(startTime, DateTime.Now, 
                        string.Empty, typeName, methodName));
                }
            }
            catch (Exception ex)
            {
                sb = new StringBuilder("Exception executing: ");
                DumpMethodCallData(typeName, methodName,methodParameters, arguments, true, sb);
                sb.AppendLine(currentUserSvc.GetCurrentUserData().ToString());
                logger.Error(sb.ToString(), ex);
                throw;
            }

            if (isTraceMethods && Config.DumpMethodReturnValue)
            {
                DumpMethodResult(typeName, returnType, returnValue, sb);
            }
            if (isTraceMethods)
            {
                logger.Info(sb.ToString());
            }
        }
        
        /// <summary>
        /// Inheriting type implements this by calling the intercepted method call. The data parameter is the value 
        /// that was originally passed to InterceptMethodCall(). Returns the value that is returned from the
        /// intercepted method call.
        /// </summary>
        /// <param name="data">The value that was originally passed to InterceptMethodCall()</param>
        /// <returns>Returns the value that is returned from the intercepted method call.</returns>
        protected abstract object ProceedWithMethodCall(object data);

        private void DumpMethodCallData(string typeName, string methodName, IList<ParameterInfo> methodParameters, 
            IList<object> arguments, bool isException, StringBuilder sb)
        {
            // Log type name and method name
            sb.AppendLine(typeName + "." + methodName);

            // Don't log params if not configured to dump params and not an exception OR params count does not match
            if ((!Config.DumpMethodParameters && !isException) || (methodParameters.Count != arguments.Count)) return;

            // Loop thru params
            for (int i = 0; i < methodParameters.Count; i++)
            {
                string paramName = methodParameters[i].Name;

                if (arguments[i] == null)
                {
                    // Argument has null value
                    sb.AppendLine(string.Format("\tparameter({1}): {0}={2}", paramName, 
                        methodParameters[i].ParameterType, "null"));
                }
                else if (IsIgnoreParam(paramName))
                {
                    // Parameter is on ignore list
                    sb.AppendLine(string.Format("\tparameter({1}): {0}={2}", paramName, 
                        methodParameters[i].ParameterType, "****"));
                }
                else
                {
                    sb.AppendLine(string.Format("\tparameter({1}): {0}={2}", paramName,
                        methodParameters[i].ParameterType, arguments[i]));
                }
            }
        }

        private bool IsIgnoreParam(string paramName)
        {
            // See if any part of the parameter name matches a name on the ignore list
            string lower = paramName.ToLower();
            return Config.ParameterNameIgnoreList.Any(ignoreParam => lower.Contains(ignoreParam.ToLower()));
        }

        private static void DumpMethodResult(string typeName, Type returnType, object returnValue, StringBuilder sb)
        {
            if (returnType.FullName == "System.Void") return;
            
            sb.Append("Return Value: ");

            string result = (returnValue == null)? "null" : returnValue.ToString();
            sb.AppendLine(result.StartsWith(typeName) ? result.Substring(typeName.Length) : result);
        }
    }
}
