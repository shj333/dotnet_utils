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
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace BerwickHeights.Platform.NHibernate.Fluent
{
    /// <summary>
    /// Static class to configure database and mappings used by NHibernate/FluentNHibernate.
    /// </summary>
    public class FluentConfigUtils
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
        /// Exposes NHibernate configuration to be able to modifiy it. 
        /// </summary>
        /// <param name="config"></param>
        public static void ExposeConfigAction(Configuration config)
        {
            //
            // Allows AOP-based transaction management. Keeps current NH session context in thread static. Calls to 
            // sessionFactory.GetCurrentSession() will look in thread static to find current session context. Look at
            // TransactionInterceptor to see how this is used. 
            //
            config.SetProperty("current_session_context_class", "thread_static");

            // TODO Get AuditIntercptor working
            // config.SetInterceptor(new AuditInterceptor(
            //    IoCContainerManagerFactory.GetIoCContainerManager().Resolve<ICurrentUserSvc>()));

            // Correct how NH handles quoting of table and column names
            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            // Update schema in database if set in app config (should only be used in dev environments)
            bool updateSchemaInDb = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>()
                .GetBooleanConfig("UpdateSchemaInDb", false);
            if (updateSchemaInDb) new SchemaExport(config).Create(false, true);
        }
    }
}
