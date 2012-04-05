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
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BerwickHeights.Platform.Core.Logging;

namespace BerwickHeights.Platform.Core.Xml
{
    /// <inheritDoc/>
    public class XmlProcessorSvc : IXmlProcessorSvc
    {
        #region Private Fields

        private readonly ILogger logger;

        private readonly XmlSchemaSet schemaSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public XmlProcessorSvc(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.GetLogger(GetType());
            schemaSet = new XmlSchemaSet();
        }

        #endregion

        #region Implementation of IXmlProcessorSvc

        /// <inheritDoc/>
        public void LoadXsdSchema(XsdDescriptor xsd)
        {
            Assembly schemaAssembly;
            try
            {
                schemaAssembly = Assembly.Load(xsd.AssemblyName);
            }
            catch (Exception e)
            {
                throw new Exception("Could not load assembly for " + xsd, e);
            }
            Stream schemaStream = schemaAssembly.GetManifestResourceStream(xsd.XsdName);
            if (schemaStream == null)
            {
                logger.Warn("Schema not found for " + xsd);
                return;
            }
            schemaSet.Add(xsd.TargetNamespace, XmlReader.Create(new StreamReader(schemaStream)));
        }

        /// <inheritDoc/>
        public string Serialize<T>(T objectInstance) where T : class
        {
            MemoryStream ms = new MemoryStream();
            new XmlSerializer(typeof(T)).Serialize(ms, objectInstance);
            ms.Seek(0L, SeekOrigin.Begin);
            return new StreamReader(ms).ReadToEnd();
        }

        /// <inheritDoc/>
        public T Deserialize<T>(string xml) where T : class
        {
            return new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml)) as T;
        }

        /// <inheritDoc/>
        public ValidationResult Validate(string xml)
        {
            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemaSet;
            ValidationCallBack callBack = new ValidationCallBack();
            settings.ValidationEventHandler += callBack.CallBack;

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            // Parse the file. 
            while (reader.Read()) { }

            ValidationResult result = new ValidationResult(callBack.ValidationErrors);
            if (logger.IsDebugEnabled) logger.Debug(result.ToString());
            return result;
        }

        #endregion

        #region Private Classes

        private class ValidationCallBack
        {
            private readonly IList<string> validationErrors = new List<string>();

            public IEnumerable<string> ValidationErrors { get { return validationErrors; } }

            public void CallBack(object sender, ValidationEventArgs e)
            {
                validationErrors.Add(e.Message);
            }
        }

        #endregion
    }
}
