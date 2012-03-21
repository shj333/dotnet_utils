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

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerwickHeights.Platform.Core.Xml
{
    /// <summary>
    /// Model class for the response to XML validation in IXmlProcessorSvc.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="validationErrors">List of validation errors, each on a new line.</param>
        public ValidationResult(IEnumerable<string> validationErrors)
        {
            ValidationErrors = validationErrors ?? new string[0];
            IsValid = (!ValidationErrors.Any());
        }

        /// <summary>
        /// Whether or not the given XML is valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// List of validation errors, each on a new line.
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; private set; }

        /// <inheritDoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string err in ValidationErrors) sb.Append("[" + err + "],");
            return "ValidationResult: " 
                + "IsValid: " + IsValid
                + ", ValidationErrors: " + sb;
        }
    }
}
