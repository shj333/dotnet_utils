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
using BerwickHeights.Platform.Core.IoC;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Castle Windsor Installer that handles the insertion of interceptors into Castle components.
    /// </summary>
    public class InterceptorsInstaller : IWindsorInstaller
    {
        #region Private Fields

        private readonly IEnumerable<InterceptorDescriptor> descriptors;

        #endregion

        #region Constructors

        /// <summary>
        /// Sets up a Castle InterceptorSelector (implements IModelInterceptorsSelector interface) which adds the 
        /// given interceptors to classes that are in the given namespaces but not in the given ignore list.
        /// </summary>
        public InterceptorsInstaller(IEnumerable<InterceptorDescriptor> descriptors)
        {
            this.descriptors = descriptors;
        }

        #endregion
        
        #region IWindsorInstaller Implementation

        /// <summary>
        /// Installs a Castle InterceptorSelector (implements IModelInterceptorsSelector interface) which adds the 
        /// given interceptors to classes that are in the given namespaces but not in the given ignore list.
        /// </summary>
        /// <param name="container">The Castle Windsor dependency injection container.</param>
        /// <param name="store">Configuration for this container.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //
            // Register each interceptor given in list of interceptors -- name them so we
            // can find them later in interceptor selector logic (see InterceptorSelector)
            //
            int idx = 0;
            foreach (InterceptorDescriptor descriptor in descriptors)
            {
                // Create component based on given interceptor type
                ComponentRegistration<IInterceptor> component =
                    Component.For<IInterceptor>().ImplementedBy(descriptor.InterceptorType);

                // Name component so we can find it later in InterceptorSelector
                component.Named(InterceptorSelector.InterceptorNamePrefix + idx);
                idx++;

                // Add configuration properties to interceptor if given
                if ((descriptor.Config != null) && (descriptor.Config.Count > 0))
                {
                    component.DependsOn(descriptor.Config);
                }

                // Register interceptor with Castle Windsor        
                container.Register(component);
            }

            // Register the interceptor selector
            container.Kernel.ProxyFactory.AddInterceptorSelector(new InterceptorSelector(descriptors));
        }

        #endregion
    }
}
