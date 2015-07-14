/*
 * Copyright 2015 Berwick Heights Software, Inc
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

using System.Configuration;
using BerwickHeights.Platform.Core.Logging;

namespace BerwickHeights.Platform.Core.Config.AppConfig
{
    /// <summary>
    /// Implementation of IConfiguration service using application configuration (ConfigurationManager) as source.
    /// </summary>
    public class ConfigurationSvc : ConfigurationSvcBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationSvc(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets configuration value for given key from application configuration (ConfigurationManager).
        /// </summary>
        /// <param name="key">Key to configuration value.</param>
        protected override string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #endregion
    }
}
