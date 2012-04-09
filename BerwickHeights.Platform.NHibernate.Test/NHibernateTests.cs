using System;
using BerwickHeights.Platform.Core.IoC;
using BerwickHeights.Platform.IoC;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Tool.hbm2ddl;
using NHCfg = NHibernate.Cfg;
using NUnit.Framework;
using ILoggerFactory = BerwickHeights.Platform.Logging;
using Log4NetLoggerFactory = BerwickHeights.Platform.Logging.Log4Net.Log4NetLoggerFactory;

namespace BerwickHeights.Platform.NHibernate.Test
{
    [TestFixture]
    public class NHibernateTests
    {
        private IIoCContainerManager container;

        [TestFixtureSetUp]
        public void Init()
        {
            container = IoCContainerManagerFactory.GetIoCContainerManager();
            container.RegisterLoggerFactory(new Log4NetLoggerFactory());

            InterceptorDescriptor descriptor = new InterceptorDescriptor(
                typeof(TransactionInterceptor), 
                null, 
                new string[] {"BerwickHeights"}, 
                new string[0]);
            container.RegisterInterceptors(descriptor);

            container.SetupNHibernateIntegration(ConfigureDatabase(), ConfigureMappings(), ExposeConfigAction, false, false);

            container.RegisterComponent(typeof(ITestDataSvc), typeof(TestDataSvc));
        }

        [Test]
        public void TestPersistEntity()
        {
            ITestDataSvc testDataSvc = container.Resolve<ITestDataSvc>();

            TestEntity entity = new TestEntity(13, "data2", "http://test.com/test/foo");
            Console.WriteLine("TestEntityId: " + entity.TestEntityId);

            testDataSvc.SaveEntity(entity);

            TestEntity entityInDb = testDataSvc.GetEntity(entity.TestEntityId);

            Console.WriteLine("Entity in db: " + entityInDb);

            Assert.AreEqual(entity.TestEntityId, entityInDb.TestEntityId);
            Assert.AreEqual(entity, entityInDb);

            // See if cache is used in detecting transactional attribute
            testDataSvc.GetEntity(entity.TestEntityId);
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
            config.SetProperty("current_session_context_class", "thread_static");
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
