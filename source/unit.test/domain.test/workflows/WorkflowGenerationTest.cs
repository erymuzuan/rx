using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowGenerationTest
    {
        public const string SYSTEM_WEB_MVC_DLL = @"c:\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll";
        public const string NEWTONSOFT_JSON_DLL = @"c:\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll";

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
        public void GenerateVariableWithDefaultValues()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string), DefaultValue = "<New application>" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ScreenActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = true, SourceCodeDirectory = @"c:\temp\sph" };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\core.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(typeof(JsonConvert).Assembly.Location);

            var result = wd.Compile(options);
            var sourceFile = Path.Combine(options.SourceCodeDirectory, "Vehicle.cs");
            var code = File.ReadAllText(sourceFile);
            foreach (var e in result.Errors)
            {
                Console.WriteLine(e.Message);
            }
            Assert.IsTrue(result.Result);
            StringAssert.Contains("public class Vehicle", code);

        }

        [Test]
        public void GenerateCsharpClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ScreenActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = false, SourceCodeDirectory = @"c:\temp\sph" };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll"));

            var code = wd.GenerateXsdCsharpClasses();
            Assert.IsNotNull(code.SingleOrDefault(x => x.Name == "Vehicle"));

        }

        [Test]
        public void GenerateJavascriptClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ScreenActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = false };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll"));


        }

        [Test]
        public void Compile()
        {

            var wd = new WorkflowDefinition
            {
                Name = "Permohonan Tanah Wakaf",
                Id = "permohonan-tanah-wakaf",
                SchemaStoreId = m_schemaStoreId
            };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });


            var apply = new ScreenActivity
            {
                Title = "Apply",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = Guid.NewGuid().ToString(),
                NextActivityWebId = Guid.NewGuid().ToString(),
                IsInitiator = true,
                Name = "Starts Screen"
            };

            wd.ActivityCollection.Add(apply);
            wd.ActivityCollection.Add(new EndActivity { WebId = apply.NextActivityWebId, Name = "Habis" });

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;
            wd.ReferencedAssemblyCollection.Add(new ReferencedAssembly { Location = SYSTEM_WEB_MVC_DLL });
            wd.ReferencedAssemblyCollection.Add(new ReferencedAssembly { Location = NEWTONSOFT_JSON_DLL });

            using (var ms = new MemoryStream())
            {
                var options = new CompilerOptions { IsDebug = true, SourceCodeDirectory = @"c:\temp\sph", Emit = true, Stream = ms };
                var result = wd.Compile(options);
                Console.WriteLine("Buidling \"{1}\" with {0} Errors", result.Errors.Count, wd.Id);
                result.Errors.ForEach(Console.WriteLine);

                Assert.IsTrue(result.Result);

                // try to instantiate the Workflow
                var assembly = Assembly.Load(ms.GetBuffer());
                var wfTypeName = string.Format("{0}.{1}", wd.CodeNamespace, wd.WorkflowTypeName);

                var wfType = assembly.GetType(wfTypeName);
                Assert.IsNotNull(wfType, wfTypeName + " is null");

                var wf = Activator.CreateInstance(wfType) as Entity;
                Assert.IsNotNull(wf);
            }

        }
    }
}