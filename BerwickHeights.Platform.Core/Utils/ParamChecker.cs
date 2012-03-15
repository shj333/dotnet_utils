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
using System.IO;

namespace BerwickHeights.Platform.Core.Utils
{
    /// <summary>
    /// Static class that checks the validity of method parameters.
    /// </summary>
    public static class ParamChecker
    {
        /// <summary>
        /// If the given string parameter is null or empty, then throws an ArgumentException.
        /// </summary>
        /// <param name="paramValue">The string value of the parameter to check</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        public static void CheckParam(string paramValue, string paramName)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                throw new ArgumentException("The parameter '" + paramName + "' cannot be null or empty");
            }
        }

        /// <summary>
        /// If the length of the given string parameter is greater than the given max length, then throws an ArgumentException.
        /// </summary>
        /// <param name="paramValue">The string value of the parameter to check</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        /// <param name="maxLength">The maximum allowed length of the string parameter</param>
        public static void CheckParamLength(string paramValue, string paramName, int maxLength)
        {
            if ((paramValue != null) && (paramValue.Length > maxLength))
            {
                throw new ArgumentException("The parameter '" + paramName + "' must be less than " + maxLength + " characters");
            }
        }

        /// <summary>
        /// If the given object parameter is null, then throws an ArgumentException.
        /// </summary>
        /// <param name="paramValue">The value of the parameter to check</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        public static void CheckParam(object paramValue, string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentException("The parameter '" + paramName + "' cannot be null");
            }
        }

        /// <summary>
        /// If the given object parameter is null or an empty list, then throws an ArgumentException.
        /// </summary>
        /// <param name="paramValue">The value of the parameter to check</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        public static void CheckParam<T>(IEnumerable<T> paramValue, string paramName)
        {
            if ((paramValue == null) || (!paramValue.Any()))
            {
                throw new ArgumentException("The parameter '" + paramName + "' cannot be null or empty");
            }
        }

        /// <summary>
        /// If the given object parameter is not greater than the given minimum, then throws an ArgumentException.
        /// </summary>
        /// <param name="paramValue">The value of the parameter to check</param>
        /// <param name="minValue">The parameter 'paramValue' must be greater than this value</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        public static void CheckParamGT(int paramValue, int minValue, string paramName)
        {
            if (paramValue <= minValue)
            {
                throw new ArgumentException("The parameter '" + paramName + "' must be greater than " + (minValue - 1));
            }
        }

        /// <summary>
        /// Checks the input stream. Throws an argument exception if the stream is null, zero length, or not readable.
        /// </summary>
        /// <param name="inputStream">The value of the parameter to check.</param>
        /// <param name="paramName">The name of the parameter (used in exception message if thrown)</param>
        public static void CheckParam(Stream inputStream, string paramName)
        {
            if (inputStream == null) throw new ArgumentException("The parameter '" + paramName + "' cannot be null");
            if (inputStream.Length == 0) throw new ArgumentException("The parameter '" + paramName + "' cannot be zero length");
            if (!inputStream.CanRead) throw new ArgumentException("The parameter '" + paramName + "' cannot be read");
        }
    }
}
