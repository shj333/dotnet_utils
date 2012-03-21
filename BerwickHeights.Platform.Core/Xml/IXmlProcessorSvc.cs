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

namespace BerwickHeights.Platform.Core.Xml
{
    /// <summary>
    /// Interface to a service that provides XML processing (serialization, deserialization, validation).
    /// </summary>
    public interface IXmlProcessorSvc : IIoCComponent
    {
        /// <summary>
        /// Loads the given XSD into the set of schemas used to validate XML. The given XSD descriptor includes
        /// the file name of the assembly where the XSD is embedded.
        /// </summary>
        /// <param name="xsd"></param>
        void LoadXsdSchema(XsdDescriptor xsd);

        /// <summary>
        /// Serialize the given object into an XML string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="objectInstance">The object to serialize.</param>
        string Serialize<T>(T objectInstance) where T : class;

        /// <summary>
        /// Deserialize the given XML string into an object of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the object that is deserialized.</typeparam>
        /// <param name="xml">The XML string to deserialize into an object.</param>
        T Deserialize<T>(string xml) where T : class;

        /// <summary>
        /// Validates the given XML against the loaded XSD schemas (see IXmlProcessorSvc.LoadXsdSchema()).
        /// </summary>
        /// <param name="xml">The XML string to validate.</param>
        ValidationResult Validate(string xml);
    }
}
