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
using CastleLogger=Castle.Core.Logging;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    ///  Implementation of ILogger using Castle's logger.
    /// </summary>
    public class Logger : ILogger
    {
        #region Private Fields

        private readonly CastleLogger.ILogger logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Logger(CastleLogger.ILogger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Implementation of ILogger

        /// <inheritDoc/>
        public bool IsDebugEnabled { get { return logger.IsDebugEnabled; } }

        /// <inheritDoc/>
        public bool IsErrorEnabled { get { return logger.IsErrorEnabled; } }

        /// <inheritDoc/>
        public bool IsFatalEnabled { get { return logger.IsFatalEnabled; } }

        /// <inheritDoc/>
        public bool IsInfoEnabled { get { return logger.IsInfoEnabled; } }

        /// <inheritDoc/>
        public bool IsWarnEnabled { get { return logger.IsWarnEnabled; } }

        /// <inheritDoc/>
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <inheritDoc/>
        public void Debug(string message, Exception exception)
        {
            logger.Debug(message, exception);
        }

        /// <inheritDoc/>
        public void Error(string message)
        {
            logger.Error(message);
        }

        /// <inheritDoc/>
        public void Error(string message, Exception exception)
        {
            logger.Error(message, exception);
        }

        /// <inheritDoc/>
        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        /// <inheritDoc/>
        public void Fatal(string message, Exception exception)
        {
            logger.Fatal(message, exception);
        }

        /// <inheritDoc/>
        public void Info(string message)
        {
            logger.Info(message);
        }

        /// <inheritDoc/>
        public void Info(string message, Exception exception)
        {
            logger.Info(message, exception);
        }

        /// <inheritDoc/>
        public void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <inheritDoc/>
        public void Warn(string message, Exception exception)
        {
            logger.Warn(message, exception);
        }

        #endregion
    }
}
