using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(BooleanExpressionCompiler))]
    public class BooleanExpressionCompiler
    {
        [ImportMany(typeof(CustomObjectSyntaxWalker), RequiredCreationPolicy = CreationPolicy.Shared, AllowRecomposition = true)]
        public CustomObjectSyntaxWalker[] MefWalkers { get; set; }


        protected CustomObjectSyntaxWalker[] Walkers
        {
            get
            {
                if (null == this.MefWalkers)
                    ObjectBuilder.ComposeMefCatalog(this);
                if (null == this.MefWalkers)
                    throw new InvalidOperationException("Cannot import MEF");
                return this.MefWalkers
                    .Distinct(new CustomObjectSyntaxWalker.Comparer())
                    .ToArray();

            }
        }

        public SnippetCompilerResult Compile(string expression, EntityDefinition entity)
        {

            if (string.IsNullOrWhiteSpace(expression))
                return new SnippetCompilerResult { Code = "true" };

            var file = new StringBuilder();
            file.AppendLine("using System;");
            file.AppendLine("using System.Linq;");
            file.AppendLine();
            file.AppendLine("namespace Bespoke." + ConfigurationManager.ApplicationName + "_" + entity.Id + ".Domain");
            file.AppendLine("{");
            file.AppendLine("   public class BooleanExpression");
            file.AppendLine("   {");
            file.AppendLinf("       public bool Evaluate({0} item, ConfigurationManager config)  ", entity.Name);
            file.AppendLine("       {");
            file.AppendLinf("           return {0};", expression);
            file.AppendLine("       }");
            file.AppendLine("   }");
            file.AppendLine("}");

            var trees = new ObjectCollection<CSharpSyntaxTree>();

            var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();
            trees.Add(tree);


            var codes = from c in entity.GenerateCode()
                        where !c.Key.EndsWith("Controller")
                        where !c.Key.EndsWith("Controller.cs")
                        let x = c.Value.Replace("using Bespoke.Sph.Web.Helpers;", string.Empty)
                        .Replace("using System.Web.Mvc;", string.Empty)
                        .Replace("using System.Linq;", string.Empty)
                        .Replace("using System.Threading.Tasks;", string.Empty)
                        select (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x);
            trees.AddRange(codes.ToArray());

            var walkersObjectModels = this.Walkers
                .Select(x => x.GetObjectModel(entity))
                .Where(x => null != x);
            trees.AddRange(walkersObjectModels);

            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReference<object>()
                .AddReference<XmlAttributeAttribute>()
                .AddReference<EntityDefinition>()
                .AddReference<EnumerableQuery>()
                .AddSyntaxTrees(trees);


            var diagnostics = compilation.GetDiagnostics();

            var result = new SnippetCompilerResult { Success = true };
            result.DiagnosticCollection.AddRange(diagnostics.Where(x => x.Id != "CS8019"));
            result.Success = result.DiagnosticCollection.Count == 0;
            result.DiagnosticCollection.ForEach(Console.WriteLine);
            if (!result.Success)
                return result;


            var statement = root.DescendantNodes().OfType<ReturnStatementSyntax>()
                .Single()
                .Expression;

            result.Code = CompileExpression(statement);
            return result;
        }


        private string CompileExpression(SyntaxNode statement)
        {
            var code = this.Walkers
                .Where(x => x.Filter(statement))
                .Select(x => x.Walk(statement))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            return code;
        }





    }


}