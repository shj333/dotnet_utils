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

using BerwickHeights.Platform.Core.IoC;

namespace BerwickHeights.Platform.Core.Config
{
    /// <summary>
    /// Interface to a service that provides application configuration.
    /// </summary>
    public interface IConfigurationSvc : IIoCComponent
    {
        /// <summary>
        /// Returns a boolean configuration value based on the given key or the given default value if the 
        /// configuration key does not match.
        /// </summary>
        bool GetBooleanConfig(string key, bool defaultVal);

        /// <summary>
        /// Returns a string configuration value based on the given key. Throws a ConfigurationErrorsException if 
        /// the given key does not match any configuration values.
        /// </summary>
        string GetStringConfig(string key);

        /// <summary>
        /// Returns a string configuration value based on the given key. Throws a ConfigurationErrorsException if 
        /// isMandatory is true and the given key does not match any configuration values.
        /// </summary>
        string GetStringConfig(string key, bool isMandatory);

        /// <summary>
        /// Returns a string array of configuration values by splitting the string value based on the given key into 
        /// a string array using a comma as a separator. Throws a ConfigurationErrorsException if the given key
        /// does not match any configuration values.
        /// </summary>
        string[] GetStringArrayConfig(string key);

        /// <summary>
        /// Returns a string array of configuration values by splitting the string value based on the given key into a 
        /// string array using a comma as a separator. Throws a ConfigurationErrorsException if isMandatory is true 
        /// and the given key does not match any configuration values.
        /// </summary>
        string[] GetStringArrayConfig(string key, bool isMandatory);

        /// <summary>
        /// Returns an integer configuration value based on the given key.
        /// Throws a ConfigurationErrorsException if isMandatory is true
        /// and the given key does not match any configuration values.
        /// </summary>
        int GetIntConfig(string key);

        /// <summary>
        /// Returns an integer configuration value based on the given key.
        /// Throws a ConfigurationErrorsException if isMandatory is true
        /// and the given key does not match any configuration values.
        /// </summary>
        int GetIntConfig(string key, bool isMandatory, int defaultVal = 0);
    }
}
