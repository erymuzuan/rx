using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class AwaitExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.AwaitExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.AwaitExpression;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var awaitExpression = (AwaitExpressionSyntax)node;
            var code = new StringBuilder();
            code.AppendLine("return " + this.GetStatementCode(model, awaitExpression.Expression));
            return code.ToString();
        }

    }
}