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
    }
}
