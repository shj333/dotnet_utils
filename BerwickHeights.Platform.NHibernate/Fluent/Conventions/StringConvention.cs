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
    /// SQL type can be overridden by the BigStringSqlType configuration key. if the property name is CreatedBy
    /// or ModifiedBy, then the type is set to UNIQUEIDENTIFIER unless overridden by the CreatedModifiedBySqlType
    /// configuration key.
    /// </summary>
    public class StringConvention : IPropertyConvention
    {
        private readonly string[] bigStringPropertyNames;
        private readonly string bigStringSqlType;
        private readonly string createdModifiedBySqlType;

        /// <summary>
        /// Constructor
        /// </summary>
        public StringConvention()
        {
            IConfigurationSvc configurationSvc = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>();
            bigStringPropertyNames = configurationSvc.GetStringArrayConfig("BigStringPropertyNames", false);
            bigStringSqlType = configurationSvc.GetStringConfig("IdSqlType", false, "NVARCHAR(MAX)");
            createdModifiedBySqlType = configurationSvc.GetStringConfig("IdSqlTypeCreatedModifiedBySqlType", false, "UNIQUEIDENTIFIER");
        }

        /// <summary>
        /// Conventions for handling string properties in FluentNHibernate. If the property name contains any of the 
        /// configured strings in BigStringPropertyNames, then the SQL type of the property is set to NVARCHAR(MAX); the
        /// SQL type can be overridden by the BigStringSqlType configuration key. if the property name is CreatedBy
        /// or ModifiedBy, then the type is set to UNIQUEIDENTIFIER unless overridden by the CreatedModifiedBySqlType
        /// configuration key.
        /// </summary>
        public void Apply(IPropertyInstance instance)
        {
            string name = instance.Name;

            if (bigStringPropertyNames.FirstOrDefault(name.Contains) != null) instance.CustomSqlType(bigStringSqlType);
            if ((name.Equals("CreatedBy")) || (name.Equals("ModifiedBy")))
            {
                instance.CustomSqlType(createdModifiedBySqlType);
                instance.CustomType(typeof (Guid));
                instance.Access.CamelCaseField();
            }
            if (name.Equals("DomainId")) instance.Unique();
            if (name.Equals("ExternalId")) instance.Unique();
        }
    }
}
