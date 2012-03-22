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

namespace BerwickHeights.Platform.Core.Xml
{
    /// <summary>
    /// Describes an embedded XSD resource that is loaded from an assembly.
    /// </summary>
    public class XsdDescriptor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblyName">The long form of the assembly name for the assembly that contains the XSD file.</param>
        /// <param name="xsdName">The case-sensitive name of the embedded XSD resource within the assembly.</param>
        /// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace 
        /// specified in the schema.</param>
        public XsdDescriptor(string assemblyName, string xsdName, string targetNamespace = null)
        {
            AssemblyName = assemblyName;
            XsdName = xsdName;
            TargetNamespace = targetNamespace;
        }

        #region Public Properties

        /// <summary>
        /// The long form of the assembly name for the assembly that contains the XSD file.
        /// </summary>
        public string AssemblyName { get; private set; }

        /// <summary>
        /// The case-sensitive name of the embedded XSD resource within the assembly.
        /// </summary>
        public string XsdName { get; private set; }

        /// <summary>
        /// The schema targetNamespace property, or null to use the targetNamespace specified in the schema.
        /// </summary>
        public string TargetNamespace { get; private set; }
        
        #endregion

        #region Overrides

        /// <inheritDoc />
        public override string ToString()
        {
            return "XsdDescriptor: "
                   + "AssemblyName: " + AssemblyName
                   + ", XsdName: " + XsdName
                   + ", TargetNamespace: " + TargetNamespace;
        }

        #endregion
    }
}
