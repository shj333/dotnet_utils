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
using BerwickHeights.Platform.IoC;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace BerwickHeights.Platform.NHibernate.Fluent
{
    /// <summary>
    /// Static class to configure database and mappings used by NHibernate/FluentNHibernate.
    /// </summary>
    public static class FluentConfigUtils
    {
        /// <summary>
        /// Configure database connection string for a SQL Server 2008 database. Turns on SQL dump in log if configured
        /// by ShowSql in configuration.
        /// </summary>
        public static IPersistenceConfigurer ConfigureSqlServer2008(string connectionStringKey)
        {
            MsSqlConfiguration config = MsSqlConfiguration.MsSql2008.ConnectionString(
                x => x.FromConnectionStringWithKey(connectionStringKey));

            // Show SQL in output if configured
            bool showSql = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>()
                .GetBooleanConfig("ShowSql", false);
            if (showSql) config.ShowSql();

            return config;
        }


        /// <summary>
        /// Sets up AOP-based transaction management, configured NH interceptors if registered in IoC container, 
        /// corrects handling of table and column name quoting by NHibernate and updates schema in database if 
        /// configuration flag UpdateSchemaInDb is set to true. Inheriting classes can override this but the
        /// override must call this base method as well. 
        /// </summary>
        /// <param name="config"></param>
        public static void ConfigureNHibernate(Configuration config)
        {
            //
            // Allows AOP-based transaction management. Keeps current NH session context in thread static. Calls to 
            // sessionFactory.GetCurrentSession() will look in thread static to find current session context. Look at
            // TransactionInterceptor to see how this is used. 
            //
            config.SetProperty("current_session_context_class", "thread_static");

            // Configure in any NHibernate interceptors registered with the IoC container
            IIoCContainerManager container = IoCContainerManagerFactory.GetIoCContainerManager();
            foreach (IInterceptor interceptor in container.ResolveAll<IInterceptor>()) config.SetInterceptor(interceptor);

            // Correct how NH handles quoting of table and column names
            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            // Update schema in database if set in app config (should only be used in dev environments)
            bool updateSchemaInDb = container.Resolve<IConfigurationSvc>().GetBooleanConfig("UpdateSchemaInDb", false);
            if (updateSchemaInDb) new SchemaExport(config).Create(false, true);
        }

        /// <summary>
        /// Sets up NHibernate caching for second level caching and query caching. The provider for the second
        /// level cache is set as given as well as the region prefix used within the cache.
        /// </summary>
        /// <typeparam name="TCacheProviderType">The type of cache provider.</typeparam>
        /// <param name="cacheSettings">The cache settings that are built.</param>
        /// <param name="regionPrefix">The prefix used in second level cache region.</param>
        public static void BuildCacheSettings<TCacheProviderType>(CacheSettingsBuilder cacheSettings, string regionPrefix) 
            where TCacheProviderType : ICacheProvider
        {
            cacheSettings
                .UseQueryCache()
                .UseSecondLevelCache()
                .ProviderClass<TCacheProviderType>()
                .RegionPrefix(regionPrefix);
        }
    }
}
