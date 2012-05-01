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
using System.Text;

namespace BerwickHeights.Platform.Core.Utils
{
    /// <summary>
    /// Static utility methods for managing lists.
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        /// Returns a string version of the given list as "[item1,item2,item3,...]".
        /// </summary>
        public static string ListToString(IEnumerable<object> list)
        {
            StringBuilder sb = new StringBuilder("[");
            foreach (object item in list) sb.Append(item + ",");
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string version of the given dictionary as "[key1={value1},key2={value2},key3={value3},...]".
        /// </summary>
        public static string DictionaryToString(IDictionary<object, object> dictionary)
        {
            StringBuilder sb = new StringBuilder("[");
            foreach (object key in dictionary.Keys) sb.Append(key + "={" + dictionary[key] + "},");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
