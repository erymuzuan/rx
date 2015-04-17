using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyActionTextFixture
    {
        [Test]
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
            StringAssert.DoesNotContain("return response", code);
            StringAssert.Contains("return 0", code);
        }
        [Test]
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
            StringAssert.DoesNotContain("return response", code);
            StringAssert.Contains("await Bespoke.Sph.TestObject.Test(", code);
        }

        [Test]
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
            StringAssert.DoesNotContain("return response", code);
            StringAssert.Contains("return 0", code);
        }

        [Test]
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

            StringAssert.Contains("return response", code);
        }
        [Test]
        public async Task CallPatientControllerValidate()
        {
            var dll = Assembly.LoadFile(@"C:\project\work\sph\bin\output\DevV1.Patient.dll");
            await Task.Delay(250);
            var action = new AssemblyAction
            {
                Title = "Validate Dob",
                Assembly = "DevV1.Patient",
                TypeName = dll.GetType("Bespoke.DevV1_patient.Domain.PatientController").FullName,
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
                Type = dll.GetType("Bespoke.DevV1_patient.Domain.Patient"),
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
  public class SampleActionTrilgger 
  {
    public async Task<object> Validate()
    {
        var item = new Bespoke.DevV1_patient.Domain.Patient
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
            StringAssert.Contains("Validate", root.ToString());


            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithUsings()
                .WithScriptClassName("TestX");

            var compiler = CSharpCompilation.Create("just.a.test.assembly")
                .WithOptions(options)
                .AddReferences(MetadataReference.CreateFromFile("Dev.Patient.dll"),
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

        [Test]
        public async Task FormatSimpleCode()
        {
            var ws = new CustomWorkspace();
            var project = ws.AddProject("test", LanguageNames.CSharp);
            await Task.Delay(500);

            const string CODE = @"public class A{
public string Name{get;set;}}";
            var tree = CSharpSyntaxTree.ParseText(CODE);

            project.AddDocument("trigger.cs", tree.GetText());


            var res = Formatter.Format(tree.GetRoot(), ws);
            Assert.AreEqual(
@"public class A
{
    public string Name { get; set; }
}", res.ToFullString());
        }
    }

}
