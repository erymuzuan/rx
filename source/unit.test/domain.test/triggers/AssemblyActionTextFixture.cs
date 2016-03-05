using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using domain.test.reports;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Xunit;

namespace domain.test.triggers
{
    
    public class AssemblyActionTextFixture
    {

        private MockRepository<EntityDefinition> m_efMock;
        private readonly MockPersistence m_persistence = new MockPersistence();

        public AssemblyActionTextFixture()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
           
        }



        [Fact]
        public async Task AssemblyActionCompile()
        {
            m_efMock.Clear();
            var jsonFile = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\Patient.json";
            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", 
                File.ReadAllText(jsonFile).DeserializeFromJson<EntityDefinition>());
            var json = File.ReadAllText($@"{ConfigurationManager.Home}\..\source\unit.test\domain.test\triggers\assembly.action.json");
            var trigger = json.DeserializeFromJson<Trigger>();
            var code = await trigger.GenerateCodeAsync();
            Console.WriteLine(code);
            var options = new CompilerOptions();
            var result = await trigger.CompileAsync(options);
            if(!result.Result)
                result.Errors.ForEach(Console.WriteLine);
            Assert.True(result.Result, result.ToString());
        }


        [Fact]
        public void ReturnTypeofTask()
        {
            var action = new AssemblyAction
            {
                IsAsyncMethod = true,
                ReturnType = typeof(Task).FullName,
                Assembly = "test",
                IsActive = true,
                Method = "TestAsync",
                TypeName = "Bespoke.Sph.Test.TestObject"
            };
            action.MethodArgCollection.Add(new MethodArg { Name = "name", Type = typeof(string), ValueProvider = new ConstantField { Value = "Erymuzuan", Type = typeof(string) } });
            action.MethodArgCollection.Add(new MethodArg { Name = "age", Type = typeof(int), ValueProvider = new DocumentField { Path = "Age", Type = typeof(int) } });
            var code = action.GeneratorCode();
            Assert.DoesNotContain("return response", code);
            Assert.Contains("return 0", code);
        }
        [Fact]
        public void StaticMethod()
        {
            var action = new AssemblyAction
            {
                IsAsyncMethod = true,
                IsStatic = true,
                ReturnType = typeof(Task).FullName,
                Assembly = "test",
                IsActive = true,
                Method = "Test",
                TypeName = "Bespoke.Sph.TestObject"
            };
            action.MethodArgCollection.Add(new MethodArg { Name = "name", Type = typeof(string), ValueProvider = new ConstantField { Value = "Erymuzuan", Type = typeof(string) } });
            action.MethodArgCollection.Add(new MethodArg { Name = "age", Type = typeof(int), ValueProvider = new DocumentField { Path = "Age", Type = typeof(int) } });
            var code = action.GeneratorCode();
            Assert.DoesNotContain("return response", code);
            Assert.Contains("await Bespoke.Sph.TestObject.Test(", code);
        }

        [Fact]
        public void VoidMethod()
        {
            var action = new AssemblyAction
            {
                IsAsyncMethod = false,
                IsVoid = true,
                Assembly = "test",
                IsActive = true,
                Method = "Test",
                TypeName = "Bespoke.Sph.TestObject"
            };
            action.MethodArgCollection.Add(new MethodArg { Name = "name", Type = typeof(string), ValueProvider = new ConstantField { Value = "Erymuzuan", Type = typeof(string) } });
            action.MethodArgCollection.Add(new MethodArg { Name = "age", Type = typeof(int), ValueProvider = new DocumentField { Path = "Age", Type = typeof(int) } });
            var code = action.GeneratorCode();
            Assert.DoesNotContain("return response", code);
            Assert.Contains("return Task.FromResult", code);
        }

        [Fact]
        public void ReturnTypeofTaskSomething()
        {
            var action = new AssemblyAction
            {
                IsAsyncMethod = true,
                ReturnType = typeof(Task<bool>).FullName,
                Assembly = "test",
                IsActive = true,
                Method = "TestAsync",
                TypeName = "Bespoke.Sph.Test.TestObject"
            };
            action.MethodArgCollection.Add(new MethodArg { Name = "name", Type = typeof(string) });
            var code = action.GeneratorCode();

            Assert.Contains("return response", code);
        }
        [Fact]
        public async Task CallPatientControllerValidate()
        {
            var patientDll = $@"{ConfigurationManager.CompilerOutputPath}\DevV1.Patient.dll";
            File.Copy(patientDll, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DevV1.Patient.dll"),true);
            var dll = Assembly.LoadFile(patientDll);
            await Task.Delay(250);
            var action = new AssemblyAction
            {
                Title = "Validate Dob",
                Assembly = "DevV1.Patient",
                TypeName = dll.GetType("Bespoke.DevV1.Patients.Domain.PatientController").FullName,
                Method = "Validate",
                IsAsyncMethod = true
            };
            action.MethodArgCollection.Add(new MethodArg
            {
                Name = "id",
                Type = typeof(string),
                ValueProvider = new ConstantField
                {
                    Name = "Dob",
                    Type = typeof(string),
                    Value = "Dob"
                }
            });
            action.MethodArgCollection.Add(new MethodArg
            {
                Name = "item",
                Type = dll.GetType("Bespoke.DevV1.Patients.Domain.Patient"),
                ValueProvider = new FunctionField
                {
                    Name = "item",
                    Script = "item"
                }
            });
            var code = @"
using Bespoke.Sph.Domain;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Dev.SampleTriggers
{
  public class SampleActionTrigger 
  {
    public async Task<object> Validate()
    {
        await Task.Delay(500);
        var item = new Bespoke.DevV1.Patients.Domain.Patient
            {
                FullName = ""Michael Scumacher"", 
                Dob = new DateTime(1965,4,6)
            };
"
                + (action.GeneratorCode())
                +
                @"
     }
  }
}";
            var tree = CSharpSyntaxTree.ParseText(code)
                ;
            var root = (CompilationUnitSyntax)tree.GetRoot().NormalizeWhitespace(indentation: "  ", elasticTrivia: true);
            Assert.Contains("Validate", root.ToString());


            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithUsings()
                .WithScriptClassName("TestX");

            var compiler = CSharpCompilation.Create("just.a.test.assembly")
                .WithOptions(options)
                .AddReferences(MetadataReference.CreateFromFile($"{ConfigurationManager.ApplicationName}.Patient.dll"),
                this.CreateMetadataReference<object>(),
                this.CreateMetadataReference<EnumerableQuery>(),
                this.CreateMetadataReference<Task>(),
                this.CreateMetadataReference<DateTime>(),
                this.CreateMetadataReference<Uri>(),
                this.CreateMetadataReference<Trigger>(),
                typeof(Controller).CreateMetadataReference())
                .AddSyntaxTrees(tree);

            var model = compiler.GetSemanticModel(tree);
            var diagnostics = compiler.GetDiagnostics();
            diagnostics.AsEnumerable().ToList()
                    .ForEach(d =>
                    {
                        Console.WriteLine("------------------------");
                        Console.WriteLine("{0} : {1}", d.Location, d.GetMessage());
                        var end = code.IndexOf("\r\n", d.Location.SourceSpan.Start, StringComparison.Ordinal);
                        var start = d.Location.SourceSpan.Start;
                        var piece = code.Substring(start, end - start);
                        Console.WriteLine(piece);

                        var symbol = model.GetEnclosingSymbol(start);
                        while (null != symbol)
                        {
                            Console.WriteLine(symbol);
                            symbol = symbol.ContainingSymbol;
                        }
                    });


            // formatter
            var res = Formatter.Format(root, new TestWorkspace());
            Console.WriteLine(res.ToFullString());

        }

        [Fact]
        public async Task FormatSimpleCode()
        {
            var ws = new AdhocWorkspace();
            var project = ws.AddProject("test", LanguageNames.CSharp);
            await Task.Delay(500);

            const string CODE = @"public class A{
public string Name{get;set;}}";
            var tree = CSharpSyntaxTree.ParseText(CODE);

            project.AddDocument("trigger.cs", tree.GetText());


            var res = Formatter.Format(tree.GetRoot(), ws);
            Assert.Equal(
@"public class A
{
    public string Name { get; set; }
}", res.ToFullString());
        }
    }

}
