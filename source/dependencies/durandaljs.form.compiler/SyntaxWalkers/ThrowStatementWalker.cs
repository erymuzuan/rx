using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ThrowStatementWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ThrowStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.ThrowStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ret = (ThrowStatementSyntax)node;
            var walker = this.GetWalker(ret.Expression);
            if (null == walker) return "throw \"what ever\";";
            var code = walker.Walk(ret.Expression, model);

            return "throw " + code + ";";
        }
    }
}