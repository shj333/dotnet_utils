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
using Castle.DynamicProxy;
using Castle.Services.Transaction;
using NHibernate;
using NHibernate.Context;
using IInterceptor = Castle.DynamicProxy.IInterceptor;
using ILoggerFactory = BerwickHeights.Platform.Core.Logging.ILoggerFactory;
using ITransaction = NHibernate.ITransaction;

namespace BerwickHeights.Platform.NHibernate
{
    /// <summary>
    /// Castle Windsor interceptor that provides transactional support to configured methods.
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        #region Private Fields

        private readonly ISessionFactory sessionFactory;
        private readonly ILogger logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public TransactionInterceptor(ISessionFactory sessionFactory,
            ILoggerFactory loggerFactory)
        {
            this.sessionFactory = sessionFactory;
            logger = loggerFactory.GetLogger(GetType());
        }

        #endregion

        #region Implementation of IInterceptor

        /// <summary>
        /// Determines if given invocation is bound to a transaction and suplies one if so.
        /// </summary>
        public void Intercept(IInvocation invocation)
        {
            if (IsInTransaction(invocation))
            {
                RunMethodInTransaction(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }

        #endregion

        #region Private Methods

        private bool IsInTransaction(IInvocation invocation)
        {
            bool result = invocation.Method.IsDefined(typeof (Transaction), false);
            if (logger.IsDebugEnabled) logger.Debug("Method is transactional?: " + result);
            return result;
        }

        private void RunMethodInTransaction(IInvocation invocation)
        {
            ISession session;
            bool isOpenedSession = false;
            try
            {
                session = sessionFactory.GetCurrentSession();
                if (logger.IsDebugEnabled) logger.Debug("Using existing session");
            }
            catch (HibernateException)
            {
                session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
                isOpenedSession = true;
                if (logger.IsDebugEnabled) logger.Debug("Created new session");
            }
            try
            {
                ITransaction transaction = (session.Transaction.IsActive) ? null : session.BeginTransaction();
                if (logger.IsDebugEnabled) logger.Debug((transaction != null) ? "Created new transaction" : "Piggybacking on existing transaction");
                try
                {
                    invocation.Proceed();
                }
                catch (Exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        if (logger.IsDebugEnabled) logger.Debug("Rolled back transaction");
                    }
                    throw;
                }

                if (transaction != null)
                {
                    transaction.Commit();
                    if (logger.IsDebugEnabled) logger.Debug("Committed transaction");
                }
            }
            finally
            {
                if (isOpenedSession)
                {
                    CurrentSessionContext.Unbind(session.SessionFactory);
                    session.Close();
                    if (logger.IsDebugEnabled) logger.Debug("Closed session");
                }
            }
        }

        #endregion
    }
}
