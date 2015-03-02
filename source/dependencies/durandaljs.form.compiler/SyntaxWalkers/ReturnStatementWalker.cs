using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ReturnStatementWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ReturnStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.ReturnStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ret = (ReturnStatementSyntax)node;
            var walker = this.GetWalker(ret.Expression);
            var code = walker.Walk(ret.Expression, model);

            return "return " + code + ";";
        }
    }
}