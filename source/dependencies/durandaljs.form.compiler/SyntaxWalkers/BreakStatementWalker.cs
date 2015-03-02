using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class BreakStatementWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.BreakStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.BreakStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            return "break;";
        }
    }
}