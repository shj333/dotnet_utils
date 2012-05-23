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
using BerwickHeights.Platform.Core.Logging;
using BerwickHeights.Platform.IoC;
using ServiceStack.Logging;

namespace BerwickHeights.Platform.ServiceStack.Logging
{
    /// <summary>
    /// ServiceStack Logger that uses the logger provided by BerwickHeights.Platform.Core interface 
    /// (BerwickHeights.Platform.Core.Logging.ILogger).
    /// </summary>
    public class BHSLogger : ILog
    {
        private readonly ILogger logger;
        private readonly string typeName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeName">Type name for object that is logging message.</param>
        public BHSLogger(string typeName)
        {
            logger = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<ILoggerFactory>().GetLogger(GetType());
            this.typeName = typeName + ": ";
        }

        #region Implementation of ServiceStack ILog

        /// <inheritDoc/>
        public void Debug(object message)
        {
            logger.Debug(typeName + message);
        }

        /// <inheritDoc/>
        public void Debug(object message, Exception exception)
        {
            logger.Debug(typeName + message, exception);
        }

        /// <inheritDoc/>
        public void DebugFormat(string format, params object[] args)
        {
            logger.Debug(typeName + string.Format(format, args));
        }

        /// <inheritDoc/>
        public void Error(object message)
        {
            logger.Error(typeName + message);
        }

        /// <inheritDoc/>
        public void Error(object message, Exception exception)
        {
            logger.Error(typeName + message, exception);
        }

        /// <inheritDoc/>
        public void ErrorFormat(string format, params object[] args)
        {
            logger.Error(typeName + string.Format(format, args));
        }

        /// <inheritDoc/>
        public void Fatal(object message)
        {
            logger.Fatal(typeName + message);
        }

        /// <inheritDoc/>
        public void Fatal(object message, Exception exception)
        {
            logger.Fatal(typeName + message, exception);
        }

        /// <inheritDoc/>
        public void FatalFormat(string format, params object[] args)
        {
            logger.Fatal(typeName + string.Format(format, args));
        }

        /// <inheritDoc/>
        public void Info(object message)
        {
            logger.Info(typeName + message);
        }

        /// <inheritDoc/>
        public void Info(object message, Exception exception)
        {
            logger.Info(typeName + message, exception);
        }

        /// <inheritDoc/>
        public void InfoFormat(string format, params object[] args)
        {
            logger.Info(typeName + string.Format(format, args));
        }

        /// <inheritDoc/>
        public void Warn(object message)
        {
            logger.Warn(typeName + message);
        }

        /// <inheritDoc/>
        public void Warn(object message, Exception exception)
        {
            logger.Warn(typeName + message, exception);
        }

        /// <inheritDoc/>
        public void WarnFormat(string format, params object[] args)
        {
            logger.Warn(typeName + string.Format(format, args));
        }

        /// <inheritDoc/>
        public bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }
        }

        #endregion
    }

}
