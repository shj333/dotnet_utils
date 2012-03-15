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

namespace BerwickHeights.Platform.Core.CurrentUser
{
    /// <summary>
    /// Provides information about the user who is currently using this thread. The information is 
    /// kept in the thread's local storage so that it can be retrieved at any point in the 
    /// architectural stack.
    /// </summary>
    public interface ICurrentUserSvc : IIoCComponent
    {
        /// <summary>
        /// Retrieves information about the user who currently using this thread. If the returned
        /// object has the Initialized property set to false, then you must not use the 
        /// retrieved information.
        /// </summary>
        CurrentUserData GetCurrentUserData();

        /// <summary>
        /// Sets the user data in the thread's local storage according to the given current user data.
        /// </summary>
        /// <param name="currentUserData">The user data to be set in thread's local storage.</param>
        void SetCurrentUserData(CurrentUserData currentUserData);

        /// <summary>
        /// Un-sets the user data in the thread's local storage so that it is no longer intialized. This is
        /// important in multi-threaded situation so that the thread's local storage is cleared out before
        /// another user begins using the thread.
        /// </summary>
        void ResetCurrentUserData();
    }
}
