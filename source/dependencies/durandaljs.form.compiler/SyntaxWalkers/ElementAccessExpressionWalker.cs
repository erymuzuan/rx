using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ElementAccessExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ElementAccessExpression }; }
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var elementAccessor = (ElementAccessExpressionSyntax) node;
            return string.Format("{0}[{1}]",
                this.EvaluateExpressionCode(elementAccessor.Expression),
                this.EvaluateExpressionCode(elementAccessor.ArgumentList.Arguments[0].Expression));
        }
    }
}