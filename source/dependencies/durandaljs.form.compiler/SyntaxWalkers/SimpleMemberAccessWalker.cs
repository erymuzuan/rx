using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.EnumerableIdentifiers;
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

        public override bool Filter(SyntaxNode node)
        {

            if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression)
                return false;

            var any = this.Walkers.Where(x => x != this)
                .Any(x => x.Filter(node));
            if (any) return false;

            return true;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ins = node as IdentifierNameSyntax;
            if (null != ins)
            {
                return "|" + ins.Identifier.Text + "|";
            }

            var maes = (MemberAccessExpressionSyntax)node;
            var local = this.EvaluateExpressionCode(maes.Expression);
            var name = this.EvaluateExpressionCode(maes.Name);
            if (string.IsNullOrWhiteSpace(local))
                return name;
            return string.Format("{0}.{1}", local, name);
        }
    }
}
