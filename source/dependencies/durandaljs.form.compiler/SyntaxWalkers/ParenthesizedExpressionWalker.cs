using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ParenthesizedExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ParenthesizedExpression }; }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var parenthesiz = node as ParenthesizedExpressionSyntax;
            if (null != parenthesiz)
            {
                return "(" + this.EvaluateExpressionCode(parenthesiz.Expression) + ")";
            }
            return string.Empty;
        }
    }
}
