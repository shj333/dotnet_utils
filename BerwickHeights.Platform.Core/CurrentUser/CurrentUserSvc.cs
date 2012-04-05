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
using BerwickHeights.Platform.Core.Logging;

namespace BerwickHeights.Platform.Core.CurrentUser
{
    /// <inheritDoc/>
    public class CurrentUserSvc : ICurrentUserSvc
    {
        #region Private Fields

        private readonly ILogger logger;

        //
        // NB: By adding the ThreadStatic attribute to these private static fields, we keep the 
        // current user's information in the thread's local storage so that it can be 
        // retrieved at any point in the architectural stack.
        //
        [ThreadStatic]
        static CurrentUserData currentUserData;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrentUserSvc(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.GetLogger(GetType());
        }

        #endregion

        #region Implementation of ICurrentUserSvc

        /// <inheritDoc/>
        public CurrentUserData GetCurrentUserData()
        {
            CurrentUserData result = currentUserData ?? (currentUserData = new CurrentUserData());
            if (logger.IsDebugEnabled) logger.Debug("Returning " + result);
            return result;
        }

        /// <inheritDoc/>
        public void SetCurrentUserData(CurrentUserData currentUserData)
        {
            if (logger.IsDebugEnabled) logger.Debug("Setting " + currentUserData);
            CurrentUserSvc.currentUserData = currentUserData;
        }

        /// <inheritDoc/>
        public void ResetCurrentUserData()
        {
            if (logger.IsDebugEnabled) logger.Debug("Resetting " + currentUserData);
            currentUserData = new CurrentUserData();
        }

        #endregion
    }
}
