using System;
using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using domain.test.reports;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowCreateEntityTest
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());

            var repos = new MockRepository<EntityDefinition>();
            repos.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", new EntityDefinition { Name = "Buidling", Id = "building" });
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(repos);
            /*
             * 
             * 
            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.Id, ed1.Name);
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
            Console.WriteLine("Cannot create type " + edTypeName);

            var reposType = sqlRepositoryType.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IRepository<>).MakeGenericType(new[] { edType });

            ObjectBuilder.AddCacheList(ff, repository);             * 
             */

        }


        [Test]
        public void Compile()
        {

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "8", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "BuidlingId", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });

            var create = new CreateEntityActivity
            {
                Name = "Create Building",
                EntityType = "Building",
                NextActivityWebId = "B",
                IsInitiator = false,
                ReturnValuePath = "BuildingId",
                WebId = "A"
            };
            create.PropertyMappingCollection.Add(new SimpleMapping { Source = "Title", Destination = "TemplateName" });
            create.PropertyMappingCollection.Add(new SimpleMapping { Source = "pemohon.Name", Destination = "Name" });
            wd.ActivityCollection.Add(create);

            var code = create.GenerateExecMethodBody(wd);
            Console.WriteLine(code);
        }
    }
}