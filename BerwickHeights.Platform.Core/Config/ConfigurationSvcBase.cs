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
using System.Configuration;
using BerwickHeights.Platform.Core.Logging;

namespace BerwickHeights.Platform.Core.Config
{
    /// <summary>
    /// Abstract base class to be used by implementations of IConfiguration.
    /// </summary>
    public abstract class ConfigurationSvcBase : IConfigurationSvc
    {
        #region Private Fields

        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger Logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected ConfigurationSvcBase(ILogger logger)
        {
            Logger = logger;
        }

        #endregion

        #region Implementation of IConfigurationSvc

        /// <inheritDoc/>
        public bool GetBooleanConfig(string key, bool defaultVal)
        {
            string cfgVal = GetValue(key);
            bool returnVal = (string.IsNullOrEmpty(cfgVal)) ? defaultVal : bool.Parse(cfgVal);
            if (Logger.IsDebugEnabled) Logger.Debug("Config " + key + ": " + cfgVal);
            return returnVal;
        }

        /// <inheritDoc/>
        public string GetStringConfig(string key, string defaultVal)
        {
            return GetStringConfig(key, true, defaultVal);
        }

        /// <inheritDoc/>
        public string GetStringConfig(string key, bool isMandatory, string defaultVal = null)
        {
            string cfgVal = GetValue(key);
            if (string.IsNullOrEmpty(cfgVal))
            {
                if ((isMandatory) && (defaultVal == null)) throw new ConfigurationErrorsException("Configuration value '" + key + "' cannot be null");
                cfgVal = defaultVal ?? string.Empty;
            }
            if (Logger.IsDebugEnabled) Logger.Debug("Config " + key + ": " + cfgVal);
            return cfgVal;
        }

        /// <inheritDoc/>
        public string[] GetStringArrayConfig(string key)
        {
            return GetStringArrayConfig(key, true);
        }

        /// <inheritDoc/>
        public string[] GetStringArrayConfig(string key, bool isMandatory)
        {
            return GetStringConfig(key, isMandatory).Split(',');
        }

        /// <inheritDoc/>
        public int GetIntConfig(string key)
        {
            return GetIntConfig(key, true);
        }

        /// <inheritDoc/>
        public int GetIntConfig(string key, bool isMandatory, int defaultVal = 0)
        {
            string valStr = GetStringConfig(key, isMandatory);
            int cfgVal;
            try
            {
                cfgVal = int.Parse(valStr);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(valStr)) throw new ConfigurationErrorsException("Configuration value (" + key + "=" + valStr + ")  must be a number", e);

                // Configuration data not available, use given default value
                cfgVal = defaultVal;
            }
            if (Logger.IsDebugEnabled) Logger.Debug("Config " + key + ": " + cfgVal);
            return cfgVal;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Implemented by inheriting class to retrieve configuration value from configuration data store.
        /// </summary>
        /// <param name="key">Key to configuration value.</param>
        protected abstract string GetValue(string key);

        #endregion
    }
}
