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
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.IoC.Castle;

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
        /// of the IoC container manager (full type name and assembly). Defaults to Castle Windsor if 
        /// application configuration data is not available for key "IoCContainerType".
        /// </summary>
        public static IIoCContainerManager GetIoCContainerManager()
        {
            lock (containerManagerLock)
            {
                if (containerManager == null)
                {
                    string containerType = ConfigurationManager.AppSettings["IoCContainerType"];
                    if (string.IsNullOrEmpty(containerType))
                    {
                        containerManager = new CastleContainerManager();
                    }
                    else
                    {
                        Type type = Type.GetType(containerType);
                        if (type == null) throw new Exception("Unknown IoC container manager type: " + containerType);
                        containerManager = Activator.CreateInstance(type) as IIoCContainerManager;
                        if (containerManager == null) throw new Exception("Configured IoC container manager type does not implement IIoCContainerManager: " + containerType);
                    }
                }

                return containerManager;
            }
        }
    }
}
