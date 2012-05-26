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
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.Core.Logging;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace BerwickHeights.Platform.IoC
{
    /// <summary>
    /// Interface to a type that manages an IoC container.
    /// </summary>
    public interface IIoCContainerManager
    {
        #region Interceptors, LoggerFactory

        /// <summary>
        /// Registers the given interceptors with the IoC container.
        /// </summary>
        /// <param name="descriptors">A list of descriptors for each interceptor.</param>
        void RegisterInterceptors(params InterceptorDescriptor[] descriptors);

        /// <summary>
        /// Registers the given logger factory instance.
        /// </summary>
        /// <param name="loggerFactory">The instance of the logger factory to register.</param>
        void RegisterLoggerFactory(ILoggerFactory loggerFactory);

        #endregion

        #region Component registration

        /// <summary>
        /// Registers any components listed in the application configuration file.
        /// </summary>
        void RegisterComponentsFromAppConfig();

        /// <summary>
        /// Registers any components listed in an external configuration file.
        /// </summary>
        /// <param name="configFile">The name of the external config file.</param>
        void RegisterComponentsFromExternalFile(string configFile);

        /// <summary>
        /// Registers components that are called in-process by looking for any types in the given assemblies that 
        /// implement an interface that inherits from IIocComponent.
        /// </summary>
        /// <param name="assemblyNames">The list of assemblies to check for types that implement an interface that
        /// inherits from IIocComponent.</param>
        void RegisterInProcComponents(params string[] assemblyNames);

        /// <summary>
        /// Registers components that are proxied to a remote component by looking for any types in the given 
        /// assemblies that implement an interface that inherits from IIocComponent.
        /// </summary>
        /// <param name="wcfServiceUrl">The URL to the remote service that is hosting the remote component.</param>
        /// <param name="assemblyNames">The list of assemblies to check for types that implement an interface that
        /// inherits from IIocComponent.</param>
        void RegisterWCFClientComponents(string wcfServiceUrl, params string[] assemblyNames);

        /// <summary>
        /// Registers components that service remote requests by looking for any types in the given assemblies that 
        /// implement an interface that inherits from IIocComponent.
        /// </summary>
        /// <param name="wcfServiceUrl">The URL of the service that is hosting the component.</param>
        /// <param name="assemblyNames">The list of assemblies to check for types that implement an interface that
        /// inherits from IIocComponent.</param>
        void RegisterWCFServiceComponents(string wcfServiceUrl, params string[] assemblyNames);

        /// <summary>
        /// Registers the given implementation type as a singleton component in the IoC container with the given
        /// service (interface) type.
        /// </summary>
        /// <param name="serviceType">The type (interface) of the component service.</param>
        /// <param name="implType">The implementation type for the component.</param>
        void RegisterComponent(Type serviceType, Type implType);

        /// <summary>
        /// Registers the given instance as a component in the IoC container. The service type and component id are 
        /// set as given.
        /// </summary>
        /// <param name="serviceType">The type (interface) of the component service.</param>
        /// <param name="instance">The instance to register as a component.</param>
        /// <param name="componentId">The id of the component.</param>
        void RegisterComponentInstance(Type serviceType, object instance, string componentId);

        #endregion

        #region NHibernate, ASP.Net MVC integration

        /// <summary>
        /// NHibernate integration.
        /// </summary>
        /// <param name="persistenceConfigurer">Sets up FluentNHibernate configuration of database type, 
        /// connection string, etc.</param>
        /// <param name="autoPersistenceModel">Auto-mappings used by FluentNHibernate.</param>
        /// <param name="setupConfig">Action that sets up configuration of NHibernate. See 
        /// FluentConfigUtils.ConfigureNHibernate().</param>
        /// <param name="setupCacheSettings">Action that sets up cache settings for NHibernate. See
        /// FluentConfigUtils.BuildCacheSettings().</param>
        /// <param name="isPerWebRequest">Determines lifestyle of NHibernate ISession component; if true, then 
        /// the lifestyle is bound to the web request; otherwise, the lifestyle is transient and the instances
        /// are destroyed when disposed.</param>
        void SetupNHibernateIntegration(IPersistenceConfigurer persistenceConfigurer,
            AutoPersistenceModel autoPersistenceModel, Action<Configuration> setupConfig,
            Action<CacheSettingsBuilder> setupCacheSettings, bool isPerWebRequest);

        /// <summary>
        /// Sets up integration between the IoC container and ASP.Net MVC so that controllers are generated by
        /// the IoC container instead of ASP.NEt MVC framework.
        /// </summary>
        void SetupASPNetMVCIntegration();

        #endregion

        #region Resolve components at runtime

        /// <summary>
        /// Returns a component that implements the given interface.
        /// </summary>
        /// <typeparam name="T">The requested component interface.</typeparam>
        T Resolve<T>();

        /// <summary>
        /// Attempts to return a component that implements the given interface; if not found in IoC container, then
        /// returns default(T).
        /// </summary>
        /// <typeparam name="T">The requested component interface.</typeparam>
        T TryResolve<T>();

        /// <summary>
        /// Returns a component that implements the given interface and has the given component id.
        /// </summary>
        /// <typeparam name="T">The requested component interface.</typeparam>
        /// <param name="componentId">The id of the requested component.</param>
        T Resolve<T>(string componentId);

        /// <summary>
        /// Returns all components that implement the given interface.
        /// </summary>
        /// <typeparam name="T">The requested component interface.</typeparam>
        IEnumerable<T> ResolveAll<T>();

        #endregion
    }
}
