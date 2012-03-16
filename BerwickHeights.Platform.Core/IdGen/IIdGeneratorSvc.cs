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

namespace BerwickHeights.Platform.Core.IdGen
{
    /// <summary>
    /// ID Generator service.
    /// </summary>
    public interface IIdGeneratorSvc : IIoCComponent
    {
        /// <summary>
        /// Returns a unique ID for the given application and application-defined object type. Uses a data store 
        /// to guarantee uniqueness of the returned id's within the application/object type space.
        /// </summary>
        /// <param name="applicationId">The unique ID of the application.</param>
        /// <param name="objectTypeId">The application-defined ID of the object type.</param>
        long GetNextId(string applicationId, int objectTypeId);
    }
}
