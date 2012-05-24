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
using BerwickHeights.Platform.Core.Logging;
using NHibernate;
using ILoggerFactory = BerwickHeights.Platform.Core.Logging.ILoggerFactory;

namespace BerwickHeights.Platform.NHibernate.DAL
{
    /// <summary>
    /// Abstract base class for DAL components.
    /// </summary>
    public abstract class PersistenceDALBase
    {
        /// <summary>
        /// Used to manage NHibernate sessions
        /// </summary>
        protected readonly ISessionFactory sessionFactory;
        /// <summary>
        /// Standard Castle logging component
        /// </summary>
        protected readonly ILogger logger;

        private readonly ICurrentUserSvc currentUserSvc;


        /// <summary>
        /// Constructor.
        /// </summary>
        protected PersistenceDALBase(ISessionFactory sessionFactory, 
            ILoggerFactory loggerFactory)
            : this(null, sessionFactory, loggerFactory)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PersistenceDALBase(ICurrentUserSvc currentUserSvc, 
            ISessionFactory sessionFactory, 
            ILoggerFactory loggerFactory)
        {
            this.currentUserSvc = currentUserSvc;
            this.sessionFactory = sessionFactory;
            logger = loggerFactory.GetLogger(GetType());
        }


        /// <summary>
        /// Get information about current user on this thread; throws exception if information is not set.
        /// </summary>
        protected CurrentUserData GetCurrentUserData()
        {
            if (currentUserSvc == null) throw new Exception("CurrentUserSvc is not set");
            CurrentUserData currentUserData = currentUserSvc.GetCurrentUserData();
            if (!currentUserData.IsInitialized)
            {
                throw new Exception("Current user data is uninitialized");
            }

            return currentUserData;
        }

        /// <summary>
        /// Adds Modified and ModifiedBy updates to HQL update query string.
        /// </summary>
        /// <param name="objectName">The name of the object that holds the Modified and ModifiedBy properties.</param>
        protected string GetAuditInfoHQL(string objectName)
        {
            return objectName + ".Modified = :modified, " + objectName + ".ModifiedBy = :modifiedBy ";
        }

        /// <summary>
        /// Sets Modified and ModifiedBy values in HQL update query. Modified value is set to now; ModifiedBy is set using CurrentUserSvc.
        /// </summary>
        /// <param name="query">The HQL update query.</param>
        protected void AddAuditInfoVals(IQuery query)
        {
            query.SetDateTime("modified", DateTime.UtcNow);
            query.SetGuid("modifiedBy", Guid.Parse(GetCurrentUserData().UserId));
        }


        /// <summary>
        /// Sets up NHibernate paging information if pageSize set to a number > 0.
        /// </summary>
        /// <param name="query">The NHibernate query.</param>
        /// <param name="pageSize">The number of database records in each page. If set to 0, then this call does nothing.</param>
        /// <param name="pageNumber">The 1-based page number.</param>
        protected void SetupPaging(IQuery query, int pageSize, int pageNumber)
        {
            if (pageSize > 0)
            {
                query.SetMaxResults(pageSize);
                query.SetFirstResult(pageSize * (pageNumber - 1));
            }
        }
    }
}
