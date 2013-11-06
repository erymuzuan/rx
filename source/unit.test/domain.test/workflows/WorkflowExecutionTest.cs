using System;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowExecutionTest
    {

        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(It.IsAny<string>()))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);
        }

        [Test]
        public void CompileAndRun()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "cd6a8751-ceed-4805-a200-02a193b651e0" };
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

            var approval = new ScreenActivity
            {
                Title = "Kelulusan",
                WebId = Guid.NewGuid().ToString(),
                ViewVirtualPath = "d"
            };
            wd.ActivityCollection.Add(approval);



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

            var wf = Activator.CreateInstance(wfType) as Entity;
            XmlSerializerService.RegisterKnowTypes(typeof(Workflow), wfType);
            Assert.IsNotNull(wf);
            
        }
    }
}
