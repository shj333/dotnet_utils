using System;
using System.Transactions;
using BerwickHeights.Platform.IoC;
using Castle.Services.Transaction;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
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
        public void TestPersistEntity()
        {
            TestEntity entity = new TestEntity(13, "data2", "http://test.com/test/foo");
            Console.WriteLine("TestEntityId: " + entity.TestEntityId);
            using (ISession session = iocContainer.Resolve<ISession>())
            {
                using (ITransaction trans = session.BeginTransaction())
                {
                    session.Save(entity);
                    trans.Commit();
                }
            }

            TestEntity entityInDb;
            using (ISession session = iocContainer.Resolve<ISession>())
            {
                using (ITransaction trans = session.BeginTransaction())
                {
                    entityInDb = session.Get<TestEntity>(entity.TestEntityId);
                    trans.Commit();
                }
            }

            Console.WriteLine("Entity in db: " + entityInDb);

            Assert.AreEqual(entity.TestEntityId, entityInDb.TestEntityId);
            Assert.AreEqual(entity, entityInDb);
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
                .Conventions.Add(
                    Table.Is(x => x.EntityType.Name), 
                    ConventionBuilder.Id.Always(x => x.CustomSqlType("uniqueidentifier")))
                .Conventions.Add<StringPropertyConvention>();
        }

        private class StringPropertyConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                if (instance.Name.Contains("Url")) instance.CustomSqlType("nvarchar(max)");
                instance.Not.Nullable();
            }
        }


        private static void ExposeConfigAction(NHCfg.Configuration config)
        {
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
            new SchemaExport(config).Create(false, true);
        }

        /// <summary>
        /// Configuration for FluentNHibernate AutoMapper
        /// </summary>
        public class AutomapConfig : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return (type == typeof(TestEntity));
            }

            public override bool IsId(Member member)
            {
                return (member.Name.Equals(member.DeclaringType.Name + "Id"));
            }
        }
    }
}
