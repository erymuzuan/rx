using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class SimpleMemberAccessExpressionWalker : OdataSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {

            if (node.Kind() != SyntaxKind.SimpleMemberAccessExpression)
                return false;

            var any = this.Walkers.Where(x => x != this)
                .Any(x => x.Filter(node));
            if (any) return false;

            return true;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = (MemberAccessExpressionSyntax)node;
            var local = this.EvaluateExpressionCode(maes.Expression);
            var name = this.EvaluateExpressionCode(maes.Name);
            if (string.IsNullOrWhiteSpace(local))
                return name;
            return string.Format("{0}.{1}", local, name);
        }
    }
}
