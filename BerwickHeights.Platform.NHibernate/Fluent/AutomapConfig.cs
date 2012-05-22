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
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace BerwickHeights.Platform.NHibernate.Fluent
{
    /// <summary>
    /// Configuration for FluentNHibernate AutoMapper that integrates with the Berwick Heights platform.
    /// </summary>
    public class AutomapConfig : DefaultAutomappingConfiguration
    {
        /// <summary>
        /// Determines if the type is a domain type entitye that is mapped by looking for domain model types that
        /// implement INHibernateEntity.
        /// </summary>
        public override bool ShouldMap(Type type)
        {
            return (typeof(INHibernateEntity).IsAssignableFrom(type));
        }

        /// <summary>
        /// Determines if the type is a domain type component by looking for domain model types that implement
        /// INHibernateComponent.
        /// </summary>
        public override bool IsComponent(Type type)
        {
            return (typeof(INHibernateComponent).IsAssignableFrom(type));
        }

        /// <summary>
        /// Makes sure that the DoNotSetModified property is not mapped (i.e., DoNotSetModified is transient).
        /// </summary>
        public override bool ShouldMap(Member member)
        {
            return (!member.Name.Equals("DoNotSetModified"))
                && base.ShouldMap(member);
        }

        /// <summary>
        /// Declares the given domain type member as the identity (primary key) if it is named SystemId.
        /// </summary>
        public override bool IsId(Member member)
        {
            return (member.Name.Equals("SystemId"));
        }
    }
}
