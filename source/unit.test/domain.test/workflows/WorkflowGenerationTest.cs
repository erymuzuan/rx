using System;
using System.IO;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowGenerationTest
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
        public void Compile()
        {

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "cd6a8751-ceed-4805-a200-02a193b651e0" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "test", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });


            var screen = new ScreenActivity
            {
                Title = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = Guid.NewGuid().ToString()
            };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            screen.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(screen);

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;
            var dll = wd.Compile(@"C:\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll",
                @"C:\project\work\sph\source\web\web.sph\bin\web.sph.dll");

            Assert.IsTrue(File.Exists(dll), "assembly " + dll);

            Console.WriteLine(screen.GetView(wd));

        }
    }
}