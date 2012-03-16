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

using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.IdGen;
using BerwickHeights.Platform.Core.Utils;
using Castle.Core.Logging;
using ServiceStack.Redis;

namespace BerwickHeights.Platform.IdGen.Redis
{
    /// <inheritDoc/>
    public class IdGeneratorSvc : IIdGeneratorSvc
    {
        #region Private Fields

        private readonly IRedisClientsManager redisManager;
        private readonly ILogger logger;

        private readonly string idKeyPrefix;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public IdGeneratorSvc(IRedisClientsManager redisManager,
            IConfigurationSvc configurationSvc, 
            ILogger logger)
        {
            this.redisManager = redisManager;
            this.logger = logger;
            idKeyPrefix = configurationSvc.GetStringConfig("IdGenerator:$Internal$:KeyPrefix", "BHS:IdGenKey:");
            idKeyPrefix = StringUtils.MustEndWith(idKeyPrefix, ":");
            logger.Info("Using " + idKeyPrefix + " as prefix for id generation keys in Redis");
        }

        #endregion

        #region Implementation of IIdGeneratorSvc

        /// <inheritDoc/>
        public long GetNextId(string applicationId, int objectTypeId)
        {
            long id;
            using (var redis = redisManager.GetClient())
            {
                id = redis.IncrementValue(idKeyPrefix + applicationId + ":" + objectTypeId);
            }
            if (logger.IsDebugEnabled) logger.Debug("Generated ID: " + id);
            return id;
        }

        #endregion
    }
}
