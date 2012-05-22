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
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace BerwickHeights.Platform.NHibernate.Fluent.Conventions
{
    /// <summary>
    /// Conventions for handling enum properties in FluentNHibernate. The SQL type is set by default to TINYINT but
    /// this can be overridden by the EnumSqlType configuration key.
    /// </summary>
    public class EnumConvention : IUserTypeConvention
    {
        private readonly string enumSqlType;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumConvention()
        {
            enumSqlType = IoCContainerManagerFactory.GetIoCContainerManager().Resolve<IConfigurationSvc>()
                .GetStringConfig("EnumSqlType", false, "TINYINT");
        }

        /// <summary>
        /// Only use with enums
        /// </summary>
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum);
        }

        /// <summary>
        /// The SQL type is set by default to TINYINT but this can be overridden by the EnumSqlType configuration key.
        /// </summary>
        public void Apply(IPropertyInstance target)
        {
            target.CustomType(target.Property.PropertyType);
            target.CustomSqlType(enumSqlType);
        }
    }
}
