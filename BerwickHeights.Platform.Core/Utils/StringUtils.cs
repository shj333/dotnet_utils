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

namespace BerwickHeights.Platform.Core.Utils
{
    /// <summary>
    /// Static utility methods for handling strings.
    /// </summary>
    public class StringUtils
    {
        /// <summary>
        /// If the given object is null, then returns string.empty. Otherwise, returns obj.ToString(). 
        /// This can be safely used to stringify an object, even if it is null.
        /// </summary>
        public static string SafeToString(object obj)
        {
            return (obj == null) ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// Returns a string that ends with the given endsWith parameter. Trims all leading and trailing 
        /// white-space characters before checking to see if the given string ends with the given
        /// endsWith parameter.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="endsWith">The string that should end the given string.</param>
        public static string MustEndWith(string str, string endsWith)
        {
            str = str.Trim();
            if (!str.EndsWith(endsWith)) endsWith += ":";
            return str;
        }
    }
}
