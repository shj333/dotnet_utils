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

namespace BerwickHeights.Platform.Core.Utils
{
    /// <summary>
    /// Utility static class for comparing DateTime objects.
    /// </summary>
    public static class DateTimeComparison
    {
        /// <summary>
        /// Compare two DateTime objects to see if they're equal excluding milliseconds.
        /// </summary>
        /// <param name="datetime1">The first DateTime</param>
        /// <param name="datetime2">The second DateTime</param>
        public static bool DateTimesAreEqualExcludingMilliseconds(DateTime datetime1, DateTime datetime2)
        {
            return (datetime1 - datetime2).Duration() < TimeSpan.FromSeconds(1);
        }
    }
}
