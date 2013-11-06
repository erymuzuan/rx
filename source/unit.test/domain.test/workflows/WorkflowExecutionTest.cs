using System;
using System.IO;
using System.Reflection;
using System.Text;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowExecutionTest
    {
        private Mock<IBinaryStore> m_store;
        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            m_store = new Mock<IBinaryStore>(MockBehavior.Strict);
            m_store.Setup(x => x.GetContent("schema-storeid"))
                .Returns(doc);
            ObjectBuilder.AddCacheList(m_store.Object);
        }

        [Test]
        public void CompileAndRun()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "schema-storeid" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });


            var pohon = new ScreenActivity
            {
                Title = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = Guid.NewGuid().ToString(),
                IsInitiator = true
            };
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(pohon);

            var decide = new DecisionActivity
            {
                WebId = Guid.NewGuid().ToString(),

            };
            wd.ActivityCollection.Add(decide);

            var approval = new ScreenActivity
            {
                Title = "Kelulusan",
                WebId = Guid.NewGuid().ToString(),
                ViewVirtualPath = "d"
            };
            wd.ActivityCollection.Add(approval);


            m_store.Setup(x => x.GetContent("wd-storeid"))
                .Returns(new BinaryStore { Content = Encoding.Unicode.GetBytes(wd.ToXmlString()), StoreId = "wd-storeid" });

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;
            var dll = wd.Compile(@"C:\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll",
                @"C:\project\work\sph\source\web\web.sph\bin\web.sph.dll");

            Assert.IsTrue(File.Exists(dll), "assembly " + dll);


            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(dll);
            var wfTypeName = string.Format("Bespoke.Sph.Workflows_{0}_{1}.{2}", wd.WorkflowDefinitionId, wd.Version,
                wd.WorkflowTypeName);

            var wfType = assembly.GetType(wfTypeName);
            Assert.IsNotNull(wfType, wfTypeName + " is null");

            var wf = Activator.CreateInstance(wfType) as Workflow;
            Assert.IsNotNull(wf);

            wf.SerializedDefinitionStoreId = "wd-storeid";
            XmlSerializerService.RegisterKnownTypes(typeof(Workflow), wfType);

            wf.StartAsync().ContinueWith(_ =>
            {
                var result = _.Result;
                Console.WriteLine(result.Status);

                wf.ExecuteAsync().Wait();
            }).Wait();

        }
    }
}
