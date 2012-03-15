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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BerwickHeights.Platform.Core.IoC
{
    /// <summary>
    /// Describes an instance of an interceptor registered in the IoC container. Interceptors are typically used for 
    /// Aspect Oriented Programming (AOP).
    /// </summary>
    public class InterceptorDescriptor
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="interceptorType">The type of this interceptor.</param>
        /// <param name="config">Interceptor-specific configuration for this instance as key-value pairs.</param>
        /// <param name="namespacePrefixes">If the full type name of a component starts with one of these 
        /// namespace prefixes, then this interceptor will intercept the component.</param>
        /// <param name="ignoredTypes">The list of full type names for components that will not be 
        /// intercepted by this interceptor. Used to prevent endless recursion in the IoC container of internal types 
        /// that should not be intercepted.</param>
        public InterceptorDescriptor(Type interceptorType, IDictionary config, IEnumerable<string> namespacePrefixes, 
            IEnumerable<string> ignoredTypes)
        {
            InterceptorType = interceptorType;
            Config = config;
            NamespacePrefixes = namespacePrefixes ?? new string[0];
            IgnoredTypes = ignoredTypes ?? new string[0];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The type of this interceptor.
        /// </summary>
        public Type InterceptorType { get; private set; }

        /// <summary>
        /// Interceptor-specific configuration for this instance as key-value pairs.
        /// </summary>
        public IDictionary Config { get; private set; }

        /// <summary>
        /// If the full type name of a component starts with one of these namespace prefixes, then this
        /// interceptor will intercept the component.
        /// </summary>
        public IEnumerable<string> NamespacePrefixes { get; private set; }

        /// <summary>
        /// The list of full type names for components that will not be intercepted by this interceptor.  
        /// Used to prevent endless recursion in the IoC container of internal types that should not be intercepted.
        /// </summary>
        public IEnumerable<string> IgnoredTypes { get; private set; }

        #endregion

        #region Overrides

        /// <inheritDoc />
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(", Config: [");
            foreach (object key in Config.Keys) sb.Append(key + "=" + Config[key] + ",");
            sb.Append("], NamespacePrefixes: [");
            foreach (string prefix in NamespacePrefixes) sb.Append(prefix + ",");
            sb.Append("], IgnoredTypes: [");
            foreach (string type in IgnoredTypes) sb.Append(type + ",");
            sb.Append("]");
            return "InterceptorDescriptor: "
                   + "InterceptorType: " + InterceptorType
                   + sb;
        }

        #endregion
    }
}
