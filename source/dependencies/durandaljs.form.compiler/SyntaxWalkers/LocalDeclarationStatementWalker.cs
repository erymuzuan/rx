using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class LocalDeclarationStatementWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.LocalDeclarationStatement }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is LocalDeclarationStatementSyntax;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var local = (LocalDeclarationStatementSyntax)node;
            var code = new StringBuilder();
            foreach (var v in local.Declaration.Variables)
            {
                var iv = v.Initializer.Value;
                if (!(iv is AwaitExpressionSyntax))
                {
                    code.AppendFormat("var {0} = ", v.Identifier.Text);
                }
                var c = base.GetStatementCode(model, iv);
                code.Append(c);
            }
            code.Append(";");
            return code.ToString();
        }
    }
}