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

using System.Collections.Generic;
using System.Linq;
using BerwickHeights.Platform.Core.Config;
using BerwickHeights.Platform.IoC;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace BerwickHeights.Platform.NHibernate.Fluent.Conventions
{
    /// <summary>
    /// Conventions for handling string properties in FluentNHibernate. If the property name contains any of the 
    /// configured strings in BigStringPropertyNames, then the SQL type of the property is set to NVARCHAR(MAX); the
    /// SQL type can be overridden by the BigStringSqlType configuration key. Also adds a unique constraint for
    /// domain ids and external ids.
    /// </summary>
    public class StringConvention : IPropertyConvention
    {
        private readonly IEnumerable<string> bigStringPropertyNames;
        private readonly string bigStringSqlType;

        /// <summary>
        /// Constructor
        /// </summary>
        public StringConvention()
        {
            IConfigurationSvc configurationSvc = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>();
            bigStringPropertyNames = configurationSvc.GetStringArrayConfig("BigStringPropertyNames", false);
            bigStringSqlType = configurationSvc.GetStringConfig("BigStringSqlType", false, "NVARCHAR(MAX)");
        }

        /// <summary>
        /// Conventions for handling string properties in FluentNHibernate. If the property name contains any of the 
        /// configured strings in BigStringPropertyNames, then the SQL type of the property is set to NVARCHAR(MAX); the
        /// SQL type can be overridden by the BigStringSqlType configuration key. Also adds a unique constraint for
        /// domain ids and external ids.
        /// </summary>
        public void Apply(IPropertyInstance instance)
        {
            // If property name contains one of the configured values, then use use special SQL type for column
            string name = instance.Name;
            if (bigStringPropertyNames.FirstOrDefault(name.Contains) != null) instance.CustomSqlType(bigStringSqlType);

            // Add unique constraint for domain ids and external ids
            if (name.Equals("DomainId")) instance.Unique();
            if (name.Equals("ExternalId")) instance.Unique();
        }
    }
}
