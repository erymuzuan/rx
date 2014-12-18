using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Host;
using NUnit.Framework;
using Roslyn.Utilities;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyActionTextFixture
    {
        [Test]
        public async Task CallPatientControllerValidate()
        {
            await Task.Delay(250);
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
            var root = (CompilationUnitSyntax)tree.GetRoot().NormalizeWhitespace(indentation: "  ", elasticTrivia: true);
            StringAssert.Contains("Validate", root.ToString());

            //  root.AddUsings(new UsingDirectiveSyntax(new CSharpSyntaxNode(), root, 0));

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
            var ws =new CustomWorkspace();
            var project = ws.AddProject("test",  LanguageNames.CSharp);
            await Task.Delay(500);

            const string CODE = @"public class A{
public string Name{get;set;}}";
            var tree = CSharpSyntaxTree.ParseText(CODE);

            project.AddDocument("trigger.cs", tree.GetText());


            var res = Formatter.Format(tree.GetRoot(), new TestWorkspace());
            Assert.AreEqual(
@"public class A
{
    public string Name { get; set; }
}", res.ToFullString());


        }
    }

    internal class TestWorkspace : Workspace
    {
        // Forces serialization of mutation calls. Must take this lock before taking stateLock.
        private readonly NonReentrantLock m_serializationLock = new NonReentrantLock();

        public TestWorkspace(HostServices hostServices = null)
            : base(hostServices ?? new CustomWorkspace().Services.HostServices, "Test")
        {
        }

        public void AddProject(ProjectId projectId, string projectName, string language = LanguageNames.CSharp)
        {
            using (this.m_serializationLock.DisposableWait())
            {
                var oldSolution = this.CurrentSolution;
                var newSolution = this.SetCurrentSolution(oldSolution.AddProject(projectId, projectName, projectName, language));

                this.RaiseWorkspaceChangedEventAsync(WorkspaceChangeKind.ProjectAdded, oldSolution, newSolution, projectId);
            }
        }

        public ProjectId AddProject(string projectName, string languageName = LanguageNames.CSharp)
        {
            ProjectId id = ProjectId.CreateNewId(debugName: projectName);
            this.AddProject(id, projectName, languageName);
            return id;
        }

        public T GetService<T>()
            where T : class, IWorkspaceService
        {
            return this.Services.GetService<T>();
        }
    }
    public class AssemblyActionWalker : CSharpSyntaxWalker
    {
        public string MethodName { get; set; }
        public AssemblyActionWalker(string methodName)
        {
            this.MethodName = methodName;
            Arguments = new List<ExpressionSyntax>();
        }

        public List<ExpressionSyntax> Arguments { get; private set; }
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {

            var member = node.Expression as MemberAccessExpressionSyntax;
            if (member != null)
            {
                var type = member.Expression as IdentifierNameSyntax;
                if (type != null && type.Identifier.Text == "k" && member.Name.Identifier.Text == this.MethodName)
                {
                    foreach (var arg in node.ArgumentList.Arguments)
                    {
                        Arguments.Add(arg.Expression);

                    }
                }
            }

            base.VisitInvocationExpression(node);
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
