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
using System.Reflection;
using log4net.Core;
using ILogger = BerwickHeights.Platform.Core.Logging.ILogger;
using L4N = log4net;

namespace BerwickHeights.Platform.Logging.Log4Net
{
    /// <summary>
    ///  Implementation of ILogger using Log4Net logger.
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        #region Private Fields

        private readonly L4N.Core.ILogger logger;
        private static readonly Type declaringType = typeof(Log4NetLogger);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Log4NetLogger(Type consumerType)
        {
            logger = L4N.LogManager.GetLogger(Assembly.GetCallingAssembly(), consumerType).Logger;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Log4NetLogger(string name)
        {
            logger = L4N.LogManager.GetLogger(Assembly.GetCallingAssembly(), name).Logger;
        }

        #endregion

        #region Implementation of ILogger

        /// <inheritDoc/>
        public bool IsDebugEnabled { get { return logger.IsEnabledFor(Level.Debug); } }

        /// <inheritDoc/>
        public bool IsErrorEnabled { get { return logger.IsEnabledFor(Level.Error); } }

        /// <inheritDoc/>
        public bool IsFatalEnabled { get { return logger.IsEnabledFor(Level.Fatal); } }

        /// <inheritDoc/>
        public bool IsInfoEnabled { get { return logger.IsEnabledFor(Level.Info); } }

        /// <inheritDoc/>
        public bool IsWarnEnabled { get { return logger.IsEnabledFor(Level.Warn); } }

        /// <inheritDoc/>
        public void Debug(string message)
        {
            Debug(message, null);
        }

        /// <inheritDoc/>
        public void Debug(string message, Exception exception)
        {
            logger.Log(declaringType, Level.Debug, message, exception);
        }

        /// <inheritDoc/>
        public void Error(string message)
        {
            Error(message, null);
        }

        /// <inheritDoc/>
        public void Error(string message, Exception exception)
        {
            logger.Log(declaringType, Level.Error, message, exception);
        }

        /// <inheritDoc/>
        public void Fatal(string message)
        {
            Fatal(message, null);
        }

        /// <inheritDoc/>
        public void Fatal(string message, Exception exception)
        {
            logger.Log(declaringType, Level.Fatal, message, exception);
        }

        /// <inheritDoc/>
        public void Info(string message)
        {
            Info(message, null);
        }

        /// <inheritDoc/>
        public void Info(string message, Exception exception)
        {
            logger.Log(declaringType, Level.Info, message, exception);
        }

        /// <inheritDoc/>
        public void Warn(string message)
        {
            Warn(message, null);
        }

        /// <inheritDoc/>
        public void Warn(string message, Exception exception)
        {
            logger.Log(declaringType, Level.Warn, message, exception);
        }

        #endregion
    }
}
