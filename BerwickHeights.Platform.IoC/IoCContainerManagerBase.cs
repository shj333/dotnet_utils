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
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.Core.Logging;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ILoggerFactory = BerwickHeights.Platform.Core.Logging.ILoggerFactory;

namespace BerwickHeights.Platform.IoC
{
    /// <summary>
    /// Abstract base class for IoC container managers.
    /// </summary>
    public abstract class IoCContainerManagerBase : IIoCContainerManager
    {
        #region Protected Methods

        /// <summary>
        /// Sets up configuration of NHibernate (database configuration via ConfigureDatabase(), mappings via 
        /// ConfigureMappings()) and exposes the configuration via supplied exposeConfigAction.
        /// </summary>
        /// <param name="persistenceConfigurer">Sets up FluentNHibernate configuration of database type, 
        /// connection string, etc.</param>
        /// <param name="autoPersistenceModel">Automappings used by FluentNHibernate.</param>
        /// <param name="logger">Logger instance in case something goes wrong.</param>
        protected virtual Configuration ConfigureNHibernate(IPersistenceConfigurer persistenceConfigurer, 
            AutoPersistenceModel autoPersistenceModel, ILogger logger)
        {
            try
            {
                return Fluently.Configure()
                  .Database(persistenceConfigurer)
                  .Mappings(m => m.AutoMappings.Add(autoPersistenceModel))
                  .ExposeConfiguration(ExposeConfigAction)
                  .BuildConfiguration();
            }
            catch (Exception e)
            {
                logger.Error("Caught exception in configuring NHibernate", e);   
                throw;
            }
        }

        /// <summary>
        /// Sets up AOP-based transaction management, configured NH interceptors if registered in IoC container, 
        /// corrects handling of table and column name quoting by NHibernate and updates schema in database if 
        /// configuration flag UpdateSchemaInDb is set to true. Inheriting classes can override this but the
        /// override must call this base method as well. 
        /// </summary>
        /// <param name="config"></param>
        protected virtual void ExposeConfigAction(Configuration config)
        {
            //
            // Allows AOP-based transaction management. Keeps current NH session context in thread static. Calls to 
            // sessionFactory.GetCurrentSession() will look in thread static to find current session context. Look at
            // TransactionInterceptor to see how this is used. 
            //
            config.SetProperty("current_session_context_class", "thread_static");

            // Configure in any NHibernate interceptors registered with the IoC container
            foreach (IInterceptor interceptor in ResolveAll<IInterceptor>()) config.SetInterceptor(interceptor);

            // Correct how NH handles quoting of table and column names
            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            // Update schema in database if set in app config (should only be used in dev environments)
            bool updateSchemaInDb = Resolve<IConfigurationSvc>()
                .GetBooleanConfig("UpdateSchemaInDb", false);
            if (updateSchemaInDb) new SchemaExport(config).Create(false, true);
        }

        #endregion

        #region Abstract implementation of IIoCContainerManager

        /// <inheritDoc/>
        public abstract void RegisterInterceptors(params InterceptorDescriptor[] descriptors);
        /// <inheritDoc/>
        public abstract void RegisterLoggerFactory(ILoggerFactory loggerFactory);
        /// <inheritDoc/>
        public abstract void RegisterComponentsFromAppConfig();
        /// <inheritDoc/>
        public abstract void RegisterComponentsFromExternalFile(string configFile);
        /// <inheritDoc/>
        public abstract void RegisterInProcComponents(params string[] assemblyNames);
        /// <inheritDoc/>
        public abstract void RegisterWCFClientComponents(string wcfServiceUrl, params string[] assemblyNames);
        /// <inheritDoc/>
        public abstract void RegisterWCFServiceComponents(string wcfServiceUrl, params string[] assemblyNames);
        /// <inheritDoc/>
        public abstract void RegisterComponent(Type serviceType, Type implType);
        /// <inheritDoc/>
        public abstract void RegisterComponentInstance(Type serviceType, object instance, string componentId);
        /// <inheritDoc/>
        public abstract void SetupNHibernateIntegration(IPersistenceConfigurer persistenceConfigurer, AutoPersistenceModel autoPersistenceModel, bool isPerWebRequest, bool isUseAutoTransactions);
        /// <inheritDoc/>
        public abstract void SetupASPNetMVCIntegration();
        /// <inheritDoc/>
        public abstract T Resolve<T>();
        /// <inheritDoc/>
        public abstract T TryResolve<T>();
        /// <inheritDoc/>
        public abstract T Resolve<T>(string componentId);
        /// <inheritDoc/>
        public abstract IEnumerable<T> ResolveAll<T>();

        #endregion
    }
}
