using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using Newtonsoft.Json;
using Xunit;


namespace domain.test.workflows
{
    public class WorkflowGenerationTest
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        [Fact]
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

        [Fact]
        public void GenerateVariableWithDefaultValues()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string), DefaultValue = "<New application>" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ReceiveActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = true, SourceCodeDirectory = @"c:\temp\sph" };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(typeof(JsonConvert).Assembly.Location);

            var result = wd.Compile(options);
            var sourceFile = Path.Combine(options.SourceCodeDirectory, "Vehicle.cs");
            var code = File.ReadAllText(sourceFile);
            foreach (var e in result.Errors)
            {
                Console.WriteLine(e.Message);
            }
            Assert.True(result.Result);
            Assert.Contains("public class Vehicle", code);

        }

        [Fact]
        public async Task WithValueObjectVariable()
        {
            var person = new ValueObjectDefinition { Name = "Person", Id = "person" };
            person.AddMember<string>("FullName", boost: 5);
            person.AddMember<string>("Titles", allowMultiple: true);
            person.AddMember<string>("Gender");
            person.AddMember<int>("Age");

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string), DefaultValue = "<New application>" });
            wd.VariableDefinitionCollection.Add(new ValueObjectVariable(person) { Name = "pemohon" });

            var receive = new ReceiveActivity
            {
                Name = "Test",
                Operation = "Daftar",
                WebId = "A",
                IsInitiator = true,
                MessagePath = "pemohon",
                NextActivityWebId = "B"
            };
            wd.ActivityCollection.Add(receive);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var cr = await wd.CompileAsync();
            Assert.True(cr.Result, cr.ToString());

        }

        [Fact]
        public void GenerateCsharpClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ReceiveActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var code = wd.GenerateCode();
            Assert.NotNull(code.SingleOrDefault(x => x.Name == "Vehicle"));

        }

        [Fact]
        public async Task GenerateJavascriptClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = "permohonan-tanah-wakaf", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ReceiveActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = false };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll"));

            var script = await wd.GenerateCustomXsdJavascriptClassAsync();
            Assert.NotNull(script);
            Assert.Contains("bespoke.sph.wf.PermohonanTanahWakafWorkflow.Vehicle", script);
            Console.WriteLine(script);


        }

        [Fact]
        public async Task Compile()
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


            var apply = new ReceiveActivity
            {
                WebId = Guid.NewGuid().ToString(),
                NextActivityWebId = Guid.NewGuid().ToString(),
                IsInitiator = true,
                Name = "Starts Screen"
            };
            wd.ActivityCollection.Add(apply);
            wd.ActivityCollection.Add(new EndActivity { WebId = apply.NextActivityWebId, Name = "Habis" });

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;


            var result = await wd.CompileAsync();
            result.Errors.ForEach(Console.WriteLine);
            Assert.True(result.Result);
            Assert.True(File.Exists(result.Output), "assembly " + result.Output);



            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(result.Output);
            var wfTypeName = $"{wd.CodeNamespace}.{wd.WorkflowTypeName}";

            var wfType = assembly.GetType(wfTypeName);
            Assert.NotNull(wfType);

            var wf = Activator.CreateInstance(wfType) as Entity;
            Assert.NotNull(wf);

        }
    }
}