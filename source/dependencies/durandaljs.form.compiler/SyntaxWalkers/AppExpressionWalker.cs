using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class AppExpressionWalker : CustomObjectSyntaxWalker
    {
        [ImportMany("ApplicationHelper", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }


        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLine("   public class ApplicationHelper");
            code.AppendLine("   {");
            code.AppendLine("       public Task<string> ShowMessageAsync(string message, string[] buttons){ return Task.FromResult(string.Empty);}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                ClassName = "ApplicationHelper",
                IdentifierText = "app"
            };
            return com;
        }

        protected override bool Filter(SymbolInfo info)
        {
            var ips = info.Symbol as IParameterSymbol;
            if (null != ips)
                return ips.Name == "app";

            var ms = info.Symbol as IMethodSymbol;
            if (null != ms)
                return ms.ContainingType.Name == "ApplicationHelper"
                    && ms.ContainingAssembly.Name == "eval";

            return false;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                var exp = this.EvaluateExpressionCode(maes.Expression);
                var name = this.EvaluateExpressionCode(maes.Name);
                if (string.IsNullOrWhiteSpace(exp))
                    return name;
                return exp + "." + name;
            }

            var id = (IdentifierNameSyntax)node;
            var text = id.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(id).ToList();
                var xp = compiler.Value.Compile(id, argumentList);
                return "app." + xp;

            }
            return string.Empty;

        }




    }
}