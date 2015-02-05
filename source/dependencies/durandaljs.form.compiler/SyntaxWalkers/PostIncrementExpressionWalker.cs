using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class PostIncrementExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.PostIncrementExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.PostIncrementExpression;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            return node.ToFullString();
        }
    }
}