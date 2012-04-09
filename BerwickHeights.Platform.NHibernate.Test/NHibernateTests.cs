using System;
using System.Transactions;
using BerwickHeights.Platform.IoC;
using Castle.Services.Transaction;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHCfg = NHibernate.Cfg;
using NUnit.Framework;
using ILoggerFactory = BerwickHeights.Platform.Logging;
using ITransaction = NHibernate.ITransaction;
using Log4NetLoggerFactory = BerwickHeights.Platform.Logging.Log4Net.Log4NetLoggerFactory;

namespace BerwickHeights.Platform.NHibernate.Test
{
    [TestFixture]
    public class NHibernateTests
    {
        private IIoCContainerManager iocContainer;

        [TestFixtureSetUp]
        public void Init()
        {
            iocContainer = IoCContainerManagerFactory.GetIoCContainerManager();
            iocContainer.RegisterLoggerFactory(new Log4NetLoggerFactory());

            iocContainer.SetupNHibernateIntegration(ConfigureDatabase(), ConfigureMappings(), ExposeConfigAction, false, true);
        }

        [Test]
        [Transaction(TransactionScopeOption.Required)]
        public void TestPersistEntity()
        {
            TestEntity entity = new TestEntity(13, "data2");
            Console.WriteLine("TestEntityId: " + entity.TestEntityId);
            using (ISession session = iocContainer.Resolve<ISession>())
            {
//                using (ITransaction trans = session.BeginTransaction())
 ///               {
                    session.SaveOrUpdate(entity);
      //              trans.Commit();
    //            }
            }
        }

        private static IPersistenceConfigurer ConfigureDatabase()
        {
            return
                MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("TestDatabase")).ShowSql();
        }

        private static AutoPersistenceModel ConfigureMappings()
        {
            return AutoMap
                .AssemblyOf<TestEntity>(new AutomapConfig())
                .Conventions.Add(Table.Is(x => x.EntityType.Name));
        }

        private static void ExposeConfigAction(NHCfg.Configuration config)
        {
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
            //new SchemaExport(config).Create(false, true);
        }

        /// <summary>
        /// Configuration for FluentNHibernate AutoMapper
        /// </summary>
        public class AutomapConfig : DefaultAutomappingConfiguration
        {
            /// <summary>
            /// Specifies the namespace where DTOs live
            /// </summary>
            public override bool ShouldMap(Type type)
            {
                return (type == typeof(TestEntity));
            }

            /// <summary>
            /// Determines whether or not the given DTO member is the identity.
            /// </summary>
            public override bool IsId(Member member)
            {
                return (member.Name.Equals(member.DeclaringType.Name + "Id"));
            }
        }
    }
}
