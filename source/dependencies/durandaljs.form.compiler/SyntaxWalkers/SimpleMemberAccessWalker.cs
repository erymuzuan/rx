using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class SimpleMemberAccessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool Filter(SymbolInfo info)
        {
            return true;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = (MemberAccessExpressionSyntax)node;
            return this.EvaluateExpressionCode(maes.Expression) + "." + this.EvaluateExpressionCode(maes.Name);
        }
    }
}
