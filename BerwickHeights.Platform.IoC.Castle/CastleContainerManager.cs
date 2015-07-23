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
using BerwickHeights.Platform.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using ILoggerFactory = BerwickHeights.Platform.Core.Logging.ILoggerFactory;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Manages the IoC container for Windsor Castle.
    /// </summary>
    public class CastleContainerManager : IoCContainerManagerBase, IDisposable
    {
        #region Private Fields

        private readonly WindsorContainer container;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CastleContainerManager()
        {
            // Create Windsor container
            container = new WindsorContainer();

            // Add the CollectionResolver so that a collection of components can be found for a given type
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
        }

        #endregion

        #region Implementation of IIoCContainerManager

        #region Interceptors, LoggerFactory

        /// <inheritDoc/>
        public override void RegisterInterceptors(params InterceptorDescriptor[] descriptors)
        {
            container.Install(new InterceptorsInstaller(descriptors));
        }

        /// <inheritDoc/>
        public override void RegisterLoggerFactory(ILoggerFactory loggerFactory)
        {
            RegisterComponentInstance(typeof(ILoggerFactory), loggerFactory, "LoggerFactory");
        }

        #endregion

        #region Component registration

        /// <inheritDoc/>
        public override void RegisterComponentsFromAppConfig()
        {
            container.Install(Configuration.FromAppConfig());
        }

        /// <inheritDoc/>
        public override void RegisterComponentsFromExternalFile(string configFile)
        {
            container.Install(Configuration.FromXmlFile(configFile));
        }

        /// <inheritDoc/>
        public override void RegisterInProcComponents(params string[] assemblyNames)
        {
            // Loop through given assembly names -- trim off leading/trailing space
            ILogger logger = container.Resolve<ILoggerFactory>().GetLogger(GetType());
            foreach (string assemblyName in assemblyNames.Select(assembly => assembly.Trim()))
            {
                try
                {
                    // Get types in assembly to see if they need to be registered
                    FromAssemblyDescriptor types = Types.FromAssemblyNamed(assemblyName);
                    
                    // Register components whose interface inherits from IIocComponent interface
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
        public override void RegisterWCFClientComponents(string wcfServiceUrl, params string[] assemblyNames)
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc/>
        public override void RegisterWCFServiceComponents(string wcfServiceUrl, params string[] assemblyNames)
        {
            throw new NotImplementedException();
        }

        /// <inheritDoc/>
        public override void RegisterComponent(Type serviceType, Type implType)
        {
            container.Register(Component.For(serviceType).ImplementedBy(implType));
        }

        /// <inheritDoc/>
        public override void RegisterComponentInstance(Type serviceType, object instance, string componentId)
        {
            container.Register(Component.For(serviceType).Named(componentId).Instance(instance));
        }

        #endregion

        #region NHibernate, ASP.Net MVC integration

        /// <inheritDoc/>
        public override void SetupNHibernateIntegration(IPersistenceConfigurer persistenceConfigurer,
            AutoPersistenceModel autoPersistenceModel, Action<NHibernate.Cfg.Configuration> setupConfig,
            Action<CacheSettingsBuilder> setupCacheSettings, bool isPerWebRequest)
        {
            ILogger logger = container.Resolve<ILoggerFactory>().GetLogger(GetType());
            ComponentRegistration<ISession> sessionCompReg = Component.For<ISession>()
                .UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession());
            if (isPerWebRequest) sessionCompReg.LifestylePerWebRequest();
            else sessionCompReg.LifestyleTransient();

            container.Kernel.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(_ => base.ConfigureNHibernate(
                    persistenceConfigurer, autoPersistenceModel, setupConfig, setupCacheSettings, logger).BuildSessionFactory()),
                sessionCompReg
            );
        }

        /// <inheritDoc/>
        public override void SetupASPNetMVCIntegration()
        {
            // If we're using ASP.Net MVC, then set up a controller factory that integrates with Castle Windsor container
            if (ControllerBuilder.Current != null)
            {
                ControllerBuilder.Current.SetControllerFactory(new CastleMVCControllerFactory(container.Kernel));
            }
        }

        #endregion

        #region Resolve components at runtime

        /// <inheritDoc/>
        public override T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        /// <inheritDoc/>
        public override T TryResolve<T>()
        {
            if (container.Kernel.HasComponent(typeof(T)))
            {
                return (T)container.Resolve(typeof(T));
            }

            return default(T);
        }

        /// <inheritDoc/>
        public override T Resolve<T>(string componentId)
        {
            return container.Resolve<T>(componentId);
        }

        /// <inheritDoc/>
        public override IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        #endregion

        #endregion

        #region Implementation of IDisposable

        /// <inheritDoc/>
        public void Dispose()
        {
            container.Dispose();
        }

        #endregion
    }
}
