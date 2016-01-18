using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.reports;
using domain.test.triggers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class QueryTest
    {
        private MockRepository<EntityDefinition> m_efMock;
        private readonly MockPersistence m_persistence = new MockPersistence();

        [SetUp]
        public void Setup()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
        }

        [Test]
        public void GenerateController()
        {
            var query = new EntityQuery { Name = "All patients", Route = "all-patients", Id = "all-patients", Entity = "Patient", WebId = "all-patients" };
            var options = new CompilerOptions();
            var sources = query.GenerateCode();
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));
        }
        [Test]
        public void GenerateWithFunctionFilter()
        {
            var query = new EntityQuery
            {
                Name = "Patients died this week",
                Route = "patients-died-this-week",
                Id = "patients-died-this-week",
                Entity = "Patient",
                WebId = "all-patients"
            };
            var lastWeek = new FunctionField {Script = "DateTime.Today.AddDays(-7)"};
            query.FilterCollection.Add(new Filter
            {
                Field = lastWeek,
                Operator = Operator.Ge,
                Term = "DeathDateTime"
            });

            var options = new CompilerOptions();
            var sources = query.GenerateCode();
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));
        }
    }
}