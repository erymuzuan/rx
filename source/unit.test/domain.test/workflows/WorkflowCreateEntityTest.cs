using System;
using System.IO;
using Bespoke.Sph.Domain;
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
        }

        [Test]
        public void Compile()
        {

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "BuidlingId", Type = typeof(int) });
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

            var code = create.GeneratedExecutionMethodCode(wd);
            Console.WriteLine(code);
        }
    }
}