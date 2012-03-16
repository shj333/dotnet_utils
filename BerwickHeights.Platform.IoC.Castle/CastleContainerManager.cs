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
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BerwickHeights.Platform.Core.IoC;
using Castle.Core.Logging;
using Castle.Facilities.AutoTx;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Manages the IoC container for Windsor Castle.
    /// </summary>
    public class CastleContainerManager : IIoCContainerManager
    {
        #region Private Fields

        private readonly WindsorContainer container;

        #endregion

        #region Constructors

        internal CastleContainerManager()
        {
            // Create Windsor container
            container = new WindsorContainer();

            // Add the CollectionResolver so that a collection of components can be found for a given type
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            // Register component for logging
            container.AddFacility(new LoggingFacility(LoggerImplementation.Log4net, "Log4Net.config"));
        }

        #endregion

        #region Implementation of IIoCContainerManager

        /// <inheritDoc/>
        public void RegisterInterceptors(IEnumerable<InterceptorDescriptor> descriptors)
        {
            container.Install(new InterceptorsInstaller(descriptors));
        }

        /// <inheritDoc/>
        public void RegisterAutomatedDBTransactions()
        {
            container.AddFacility(new AutoTxFacility());
        }

        /// <inheritDoc/>
        public void RegisterComponentsFromAppConfig()
        {
            container.Install(Configuration.FromAppConfig());
        }

        /// <inheritDoc/>
        public void RegisterComponentsFromExternalFile(string configFile)
        {
            container.Install(Configuration.FromXmlFile(configFile));
        }

        /// <inheritDoc/>
        public void RegisterInProcComponents(IEnumerable<string> assemblyNames)
        {
            // Loop through given assembly names -- trim off leading/trailing space
            ILogger logger = container.Resolve<ILogger>();
            foreach (string assemblyName in assemblyNames.Select(assembly => assembly.Trim()))
            {
                try
                {
                    // Get types in assembly to see if they need to be registered
                    FromAssemblyDescriptor types = AllTypes.FromAssemblyNamed(assemblyName);
                    
                    // Register components whose interface inherits from Virtify-custom IIocComponent interface
                    container.Register(types.BasedOn<IIoCComponent>()
                        .WithService
                        .FromInterface());
                }
                catch (Exception e)
                {
                    logger.Error("Could not register types in assembly '" + assemblyName + "'", e);
                }
            }
        }

        /// <inheritDoc/>
        public void RegisterWCFClientComponents(IEnumerable<string> assemblyNames, string wcfServiceUrl)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritDoc/>
        public void RegisterWCFServiceComponents(IEnumerable<string> assemblyNames, string wcfServiceUrl)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritDoc/>
        public void RegisterComponent(Type serviceType, Type implType)
        {
            container.Register(Component.For(serviceType).ImplementedBy(implType));
        }

        /// <inheritDoc/>
        public void RegisterComponentInstance(Type serviceType, object instance, string componentId)
        {
            container.Register(Component.For(serviceType).Named(componentId).Instance(instance));
        }

        /// <inheritDoc/>
        public void SetupASPNetMVCIntegration()
        {
            // If we're using ASP.Net MVC, then set up a controller factory that integrates with Castle Windsor container
            if (ControllerBuilder.Current != null)
            {
                ControllerBuilder.Current.SetControllerFactory(new CastleMVCControllerFactory(container.Kernel));
            }
        }

        /// <inheritDoc/>
        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        /// <inheritDoc/>
        public T Resolve<T>(string componentId)
        {
            return container.Resolve<T>(componentId);
        }

        /// <inheritDoc/>
        public IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        #endregion
    }
}
