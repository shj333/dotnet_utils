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

namespace BerwickHeights.Platform.Core.CurrentUser
{
    /// <summary>
    /// Holds information about the user who is currently using this thread. The information is 
    /// kept in the thread's local storage so that it can be retrieved at any point in the 
    /// architecture stack.
    /// </summary>

    public class CurrentUserData
    {
        #region Private Fields

        private readonly IDictionary<string, string> attributes; 

        #endregion

        #region Constructors

        internal CurrentUserData() : this(string.Empty, string.Empty, string.Empty, null)
        {
        }

        /// <summary>
        /// Constructor for cases where current user has no additional attributes.
        /// </summary>
        public CurrentUserData(string userId, string userName, string sessionId)
            : this(userId, userName, sessionId, null)
        {
        }

        /// <summary>
        /// Constructor for user that has additional attributes.
        /// </summary>
        public CurrentUserData(string userId, string userName, string sessionId, IDictionary<string, string> attributes)
        {
            IsInitialized = !string.IsNullOrEmpty(userId);
            UserId = userId;
            UserName = userName;
            SessionId = sessionId;
            OperationId = (IsInitialized) ? Guid.NewGuid().ToString() : string.Empty;
            this.attributes = attributes ?? new Dictionary<string, string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique ID of the user generated by the authentication system.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// The name of the current user (used for display purposes).
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// This user's session.
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// The operation ID is used to group all operations that occur in a single transaction.
        /// </summary>
        public string OperationId { get; private set; }

        /// <summary>
        /// Returns the attribute for this user that matches the given key or an empty string if no match is found.
        /// </summary>
        public string GetAttribute(string key)
        {
            return (attributes.ContainsKey(key)) ? attributes[key] : string.Empty;
        }

        /// <summary>
        /// Returns whether or not the attributes list for this user contains the given key.
        /// </summary>
        public bool HasAttribute(string key)
        {
            return attributes.ContainsKey(key);
        }

        /// <summary>
        /// Whether or not the user information has been initialized for this thread. If the 
        /// value of this property is false, then the data in this object must not be 
        /// used.
        /// </summary>
        public bool IsInitialized { get; private set; }

        #endregion

        #region Overrides

        /// <inheritDoc />
        public override string ToString()
        {
            return "CurrentUserData: "
                   + "UserId: " + UserId
                   + ", UserName: " + UserName
                   + ", SessionId: " + SessionId
                   + ", OperationId: " + OperationId
                   + ", IsInitialized: " + IsInitialized;
        }

        #endregion
    }
}
