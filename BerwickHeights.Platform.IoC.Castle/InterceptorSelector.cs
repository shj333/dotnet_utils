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
using BerwickHeights.Platform.Core.IoC;
using Castle.Core;
using Castle.MicroKernel.Proxy;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Adds configured Castle Windsor interceptors to components matching configured parameters.
    /// </summary>
    public class InterceptorSelector : IModelInterceptorsSelector
    {
        #region Private Fields

        private readonly IList<InterceptorDescriptor> descriptors;

        /// <summary>
        /// Prefix for the names of the interceptors.
        /// </summary>
        internal const string InterceptorNamePrefix = "BHS_INTERCEPTOR_";
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        internal InterceptorSelector(IEnumerable<InterceptorDescriptor> descriptors)
        {
            this.descriptors = descriptors.ToList();
        }
        
        #endregion
        
        #region IModelInterceptorsSelector Implementation
        
        /// <inheritDoc/>
        public bool HasInterceptors(ComponentModel model)
        {
            // Don't add interceptors to themselves
            return (!model.Name.StartsWith(InterceptorNamePrefix));
        }

        /// <inheritDoc/>
        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] refs)
        {
            //
            // Add interceptors that have been otherwise configured for this component (e.g., through an attribute 
            // on the class)
            //
            List<InterceptorReference> interceptors = new List<InterceptorReference>();
            interceptors.AddRange(model.Interceptors);

            // Add the interceptors that are configured for this selector
            int idx = 0;
            foreach (InterceptorDescriptor descriptor in descriptors)
            {
                // Don't add interceptors to classes on the ignore list
                string modelFullClassName = model.Implementation.Namespace + "." + model.Implementation.Name;
                if (descriptor.IgnoredTypes.Any(fullClassName => (modelFullClassName).Equals(fullClassName))) continue;

                // If the model's namespace matches any of the given namespace prefixes, then add the interceptor to it
                if (model.Implementation.Namespace == null) continue;
                if (descriptor.NamespacePrefixes.Any(prefix => model.Implementation.Namespace.StartsWith(prefix)))
                {
                    // Locate interceptor by index using name set in InterceptorsInstaller
                    interceptors.Add(InterceptorReference.ForKey(InterceptorNamePrefix + idx));
                }
                idx++;
            }

            return interceptors.ToArray();
        }

        #endregion
    }
}
