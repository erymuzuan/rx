using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ContinueStatementWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ContinueStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.ContinueStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            return "continue;";
        }
    }
}