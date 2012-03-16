﻿/*
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

using System.Configuration;
using BerwickHeights.Platform.Core.Config;
using Castle.Core.Logging;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.Config.Redis
{
    /// <summary>
    /// Implementation of IConfiguration service using Redis as a data store. All configuration keys are prefixed
    /// by "BHS:ConfigKey" unless this prefix is overridden in DotNet application configuration using the 
    /// "BHSConfigKeyPrefix" key in application configuration.
    /// </summary>
    public class ConfigurationSvc : ConfigurationSvcBase
    {
        #region Private Fields

        private readonly IRedisClientsManager redisManager;
        private readonly ILogger logger;

        private readonly string configKeyPrefix;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationSvc(IRedisClientsManager redisManager,
            ILogger logger)
        {
            this.redisManager = redisManager;
            this.logger = logger;
            string tmp = ConfigurationManager.AppSettings["BHSConfigKeyPrefix"];
            configKeyPrefix = (!string.IsNullOrEmpty(tmp)) ? tmp : "BHS:ConfigKey:";
            logger.Info("Using " + configKeyPrefix + " as prefix for configuration keys in Redis");
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets configuration value for given key from Redis data store.
        /// </summary>
        /// <param name="key">Key to configuration value.</param>
        protected override string GetValue(string key)
        {
            using (var redis = redisManager.GetReadOnlyClient())
            {
                string cfgVal = redis.GetValue(configKeyPrefix + key);
                if (logger.IsDebugEnabled) logger.Debug("Config " + key + ": " + cfgVal);
                return cfgVal;
            }
        }

        #endregion
    }
}
