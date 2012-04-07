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

using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace BerwickHeights.Platform.IoC
{
    /// <summary>
    /// Abstract base class for IoC container managers.
    /// </summary>
    public abstract class IoCContainerManagerBase
    {
        #region Protected Methods

        /// <summary>
        /// Sets up configuration of NHibernate (database configuration via ConfigureDatabase(), mappings via 
        /// ConfigureMappings()). Also exposes the configuration to the database instance so that the latest 
        /// schema is applied (via ConfigurationAction()).
        /// </summary>
        /// <param name="persistenceConfigurer">Sets up FluentNHibernate configuration of database type, 
        /// connection string, etc.</param>
        /// <param name="autoPersistenceModel">Automappings used by FluentNHibernate.</param>
        protected virtual Configuration ConfigureNHibernate(IPersistenceConfigurer persistenceConfigurer, 
            AutoPersistenceModel autoPersistenceModel)
        {
            return Fluently.Configure()
              .Database(persistenceConfigurer)
              .Mappings(m => m.AutoMappings.Add(autoPersistenceModel))
              .ExposeConfiguration(ConfigurationAction)
              .BuildConfiguration();
        }

        /// <summary>
        /// Exposes the configuration to the database instance so that the latest schema is applied.
        /// </summary>
        /// <param name="config"></param>
        protected virtual void ConfigurationAction(Configuration config)
        {
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
        }

        #endregion
    }
}
