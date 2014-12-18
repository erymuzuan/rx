using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyActionTextFixture
    {
        [Test]
        public void CallPatientControllerValidate()
        {
            var action = new AssemblyAction
            {
                Title = "Validate Dob",
                Assembly = "Dev.Patient",
                TypeName = "Bespoke.Dev_patient.Domain.PatientController",
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
                TypeName = "Bespoke.Dev_patient.Domain.Patient, Dev.Patient",
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
        var item = new Bespoke.Dev_patient.Domain.Patient
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
            var root = (CompilationUnitSyntax)tree.GetRoot().NormalizeWhitespace(indentation:"  ", elasticTrivia:true);
            StringAssert.Contains("Validate", root.ToString());

            //  root.AddUsings(new UsingDirectiveSyntax(new CSharpSyntaxNode(), root, 0));

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithUsings()
                .WithScriptClassName("TestX");

            var compiler = CSharpCompilation.Create("mine")
                .WithOptions(options)
                .AddReferences(MetadataReference.CreateFromFile("Dev.Patient.dll"),
                this.CreateMetadataReference<object>(),
                this.CreateMetadataReference<EnumerableQuery>(),
                this.CreateMetadataReference<Task>(),
                this.CreateMetadataReference<DateTime>(),
                this.CreateMetadataReference<Uri>(),
                this.CreateMetadataReference<Trigger>(),
                typeof(System.Web.Mvc.Controller).CreateMetadataReference())
                .AddSyntaxTrees(tree);
            using (var stream = new FileStream("mine.dll", FileMode.Create))
            {
                var result = compiler.Emit(stream);
                result.Diagnostics.AsEnumerable().ToList()
                    .ForEach(d =>
                    {
                        Console.WriteLine("------------------------");
                        Console.WriteLine("{0} : {1}\r\n{2}", d.Location, d.GetMessage(), d.Category);

                    });
                Assert.IsTrue(result.Success);
            }

        }
    }

    public static class CompilerHelper
    {
        public static MetadataReference CreateMetadataReference(this Type type)
        {
            return MetadataReference.CreateFromAssembly(type.Assembly);
        }
        public static MetadataReference CreateMetadataReference<T>(this object type)
        {
            return MetadataReference.CreateFromAssembly((typeof(T)).Assembly);
        }
    }
}
