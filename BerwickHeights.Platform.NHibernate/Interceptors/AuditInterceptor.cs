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
using BerwickHeights.Platform.Core.CurrentUser;
using BerwickHeights.Platform.Core.Model;
using NHibernate;
using NHibernate.Type;

namespace BerwickHeights.Platform.NHibernate.Interceptors
{
    /// <summary>
    /// NHibernate interceptor that fills in the Created, CreatedBy, Modified and
    /// ModifiedBy columns before the record is written to the database.
    /// </summary>
    public class AuditInterceptor : EmptyInterceptor
    {
        private readonly ICurrentUserSvc currentUserSvc;


        /// <summary>
        /// Constructor.
        /// </summary>
        public AuditInterceptor(ICurrentUserSvc currentUserSvc)
        {
            this.currentUserSvc = currentUserSvc;
        }


        /// <inheritDoc/>
        public override bool OnFlushDirty(object entity, object id, object[] currentState,
            object[] previousState, string[] propertyNames, IType[] types)
        {
            return SetAuditInfo(entity, currentState, propertyNames);
        }

        /// <inheritDoc/>
        public override bool OnSave(object entity, object id, object[] state,
            string[] propertyNames, IType[] types)
        {
            return SetAuditInfo(entity, state, propertyNames);
        }

        private bool SetAuditInfo(object entity, object[] state, string[] propertyNames)
        {
            // Get user info from authentication system
            CurrentUserData curUserData = currentUserSvc.GetCurrentUserData();
            Guid curUserId = (curUserData.IsInitialized) ? Guid.Parse(curUserData.UserId) : Guid.Empty;

            // See if setting Modified and ModifiedBy has been turned off by the caller
            AuditInfoBase info = entity as AuditInfoBase;
            bool doNotSetModified = ((info != null) && (info.DoNotSetModified));

            AuditInfoBase auditInfo = entity as AuditInfoBase;
            if (auditInfo != null)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    // If Created field not set yet, then set to now (this is an INSERT)
                    if (("Created" == propertyNames[i]) && ((DateTime)state[i] == DateTime.MinValue))
                    {
                        state[i] = DateTime.UtcNow;
                    }
                    // Set CreatedBy to current user if not set yet and user info is available
                    else if (("CreatedBy" == propertyNames[i]) && ((Guid)state[i] == Guid.Empty) && (curUserData.IsInitialized))
                    {
                        state[i] = curUserId;
                    }
                    // Set Modified field to now (this is an INSERT or UPDATE)
                    else if (("Modified" == propertyNames[i]) && (!doNotSetModified))
                    {
                        state[i] = DateTime.UtcNow;
                    }
                    // Set ModifiedBy to current user if available
                    else if (("ModifiedBy" == propertyNames[i]) && (!doNotSetModified) && (curUserData.IsInitialized))
                    {
                        state[i] = curUserId;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
