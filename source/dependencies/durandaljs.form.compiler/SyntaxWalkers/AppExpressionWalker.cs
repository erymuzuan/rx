using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
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
            get { return new[] { SyntaxKind.InvocationExpression }; }
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

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // NOTE : calling this.Evaluate or this.GetArguments will reset this.Code
            var code = this.Code.ToString();
            var text = node.Identifier.Text;

            //var sb = new StringBuilder();

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node).ToList();
                var xp = compiler.Value.Compile(node, argumentList);
                this.Code.Clear();
                this.Code.Append(code);
                if (string.IsNullOrWhiteSpace(code))
                    this.Code.Append("app." + xp);
                else
                    this.Code.Append("." + xp);
            }



            base.VisitIdentifierName(node);
        }




    }
}