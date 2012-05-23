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
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace BerwickHeights.Platform.NHibernate.Fluent.Conventions
{
    /// <summary>
    /// Sets identities (primary keys) in FluentNHibernate mappings to use UNIQUEIDENTIFIER as the SQL type
    /// but this can be overridden by the IdSqlType configuration key. Sets the GuidComb algorithm to be used
    /// to generate new identifiers (see Jimmy Nilsson's article at 
    /// http://www.informit.com/articles/article.aspx?p=25862&amp;seqNum=7).
    /// </summary>
    public class IdConvention : IIdConvention
    {
        /// <summary>
        /// If identity is a Guid, then sets the GuidComb algorithm to be used to generate new identifiers (see 
        /// Jimmy Nilsson's article at 
        /// http://www.informit.com/articles/article.aspx?p=25862&amp;seqNum=7).
        /// </summary>
        public void Apply(IIdentityInstance instance)
        {
            Type propertyType = instance.Property.PropertyType;
            if ((propertyType == typeof(Guid)) || (propertyType == typeof(Guid?))) instance.GeneratedBy.GuidComb();
        }
    }
}
