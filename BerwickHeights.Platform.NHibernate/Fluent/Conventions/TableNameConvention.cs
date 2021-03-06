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

using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.IoC;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace BerwickHeights.Platform.NHibernate.Fluent.Conventions
{
    /// <summary>
    /// Sets table names in FluentNHibernate to the class name prefixed by the value in the SqlTableNamePrefix
    /// configuration key. 
    /// </summary>
    public class TableNameConvention : IClassConvention
    {
        private readonly string tableNamePrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        public TableNameConvention()
        {
            tableNamePrefix = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>()
                .GetStringConfig("SqlTableNamePrefix", true);
            if (!tableNamePrefix.EndsWith("_")) tableNamePrefix += "_";
        }

        /// <summary>
        /// Sets table names in FluentNHibernate to the class name prefixed by the value in the SqlTableNamePrefix
        /// configuration key. 
        /// </summary>
        public void Apply(IClassInstance instance)
        {
            string nameSpace = instance.EntityType.Namespace;
            if ((!string.IsNullOrEmpty(nameSpace)) && (nameSpace.StartsWith("BerwickHeights.Platform.PerfTest")))
            {
                instance.Table("PERF_" + instance.EntityType.Name);
            }
            else
            {
                instance.Table(tableNamePrefix + instance.EntityType.Name);
            }
        }
    }
}
