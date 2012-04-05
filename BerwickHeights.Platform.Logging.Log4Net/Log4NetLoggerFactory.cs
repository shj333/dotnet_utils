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
using System.IO;
using BerwickHeights.Platform.Core.Logging;
using log4net.Config;

namespace BerwickHeights.Platform.Logging.Log4Net
{
    /// <summary>
    /// Factory that produces instances of loggers using Log4Net. Should be a singleton that is dependency
    /// injected into consumers.
    /// </summary>
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Constructor. Configures Log4Net using the supplied configuration file. If config file name is not rooted,
        /// then assumes the file is relative to base directory of current app domain.
        /// </summary>
        /// <param name="configFile">Name of the Log4Net config file.</param>
        public Log4NetLoggerFactory(string configFile)
        {
            FileInfo fileInfo = (Path.IsPathRooted(configFile)) 
                ? new FileInfo(configFile) 
                : new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            if (!fileInfo.Exists) throw new Exception("Cannot locate Log4Net config file: " + configFile);
            XmlConfigurator.Configure(fileInfo);
        }

        /// <summary>
        /// Constructor that uses a Log4Net config file named Log4Net.config
        /// </summary>
        public Log4NetLoggerFactory() : this("Log4Net.config")
        {
        }

        #region Implementation of ILoggerFactory

        /// <inheritDoc/>
        public ILogger GetLogger(Type consumerType)
        {
            return new Log4NetLogger(consumerType);
        }

        /// <inheritDoc/>
        public ILogger GetLogger(string name)
        {
            return new Log4NetLogger(name);
        }

        #endregion
    }
}
