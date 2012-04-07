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
using System.Configuration;

namespace BerwickHeights.Platform.IoC
{
    /// <summary>
    /// Static class factory to get singleton instance of the IoC container manager.
    /// </summary>
    public static class IoCContainerManagerFactory
    {
        private static IIoCContainerManager containerManager;
        private static readonly object containerManagerLock = new object();

        /// <summary>
        /// Returns the singleton instance of the IoC container manager configured in application configuration 
        /// according to the key "IoCContainerType". The configured value should be the fully qualified type name 
        /// of the IoC container manager (full type name and assembly).
        /// </summary>
        public static IIoCContainerManager GetIoCContainerManager()
        {
            return GetIoCContainerManager(null);
        }

        /// <summary>
        /// Returns the singleton instance of the IoC container manager specified by containerType. If containerType
        /// is empty, then uses the container manager configured in application configuration according to the key 
        /// "IoCContainerType". The configured value should be the fully qualified type name of the IoC container 
        /// manager (full type name and assembly).
        /// </summary>
        /// <param name="containerType">The fully qualified type name of the IoC container manager (full type name 
        /// and assembly).</param>
        public static IIoCContainerManager GetIoCContainerManager(string containerType)
        {
            lock (containerManagerLock)
            {
                if (containerManager == null)
                {
                    if (string.IsNullOrEmpty(containerType)) containerType = ConfigurationManager.AppSettings["IoCContainerType"];
                    if (string.IsNullOrEmpty(containerType)) throw new Exception("Configuration missing for IoC container type (IoCContainerType)");

                    Type type = Type.GetType(containerType);
                    if (type == null) throw new Exception("Unknown IoC container manager type: " + containerType);
                    containerManager = Activator.CreateInstance(type) as IIoCContainerManager;
                    if (containerManager == null) throw new Exception("Configured IoC container manager type does not implement IIoCContainerManager: " + containerType);
                }

                return containerManager;
            }
        }
    }
}
