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
    /// Describes an embedded XSD file that is loaded by IXmlProcessorSvc.
    /// </summary>
    public class XsdDescriptor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblyFileName">The file name of the assembly where the XSD file is embedded.</param>
        /// <param name="xsdFileName">The file name of the embedded XSD file. Note that this is the key used to find 
        /// the embedded resource within the assembly, so it may be more than just the original file name.</param>
        /// <param name="targetNamespace">The schema targetNamespace property, or null to use the targetNamespace 
        /// specified in the schema.</param>
        public XsdDescriptor(string assemblyFileName, string xsdFileName, string targetNamespace = null)
        {
            AssemblyFileName = assemblyFileName;
            XsdFileName = xsdFileName;
            TargetNamespace = targetNamespace;
        }

        #region Public Properties

        /// <summary>
        /// The file name of the assembly where the XSD file is embedded.
        /// </summary>
        public string AssemblyFileName { get; private set; }

        /// <summary>
        /// The file name of the embedded XSD file. Note that this is the key used to find the embedded
        /// resource within the assembly, so it may be more than just the original file name.
        /// </summary>
        public string XsdFileName { get; private set; }

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
                   + "AssemblyFileName: " + AssemblyFileName
                   + ", XsdFileName: " + XsdFileName
                   + ", TargetNamespace: " + TargetNamespace;
        }

        #endregion
    }
}
