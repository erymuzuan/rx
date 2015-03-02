using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ExpressionStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ExpressionStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.ExpressionStatement;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ess = (ExpressionStatementSyntax)node;
            var w = this.GetWalker(ess.Expression);
            if (null == w) return string.Empty;
            var code = w.Walk(ess.Expression, model);
            var semiColon = (string.Format("{0}", code).TrimEnd().EndsWith(";") ? "" : ";");
            return code + semiColon;
        }
    }
}