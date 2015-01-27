using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ExpressionStatementWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ExpressionStatement }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is ExpressionStatementSyntax;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ess = (ExpressionStatementSyntax)node;
            var code = this.Walkers
                   .Where(x => x.Filter(ess.Expression, model))
                   .Select(x => x.Walk(ess.Expression, model))
                   .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            var semiColon = (string.Format("{0}", code).TrimEnd().EndsWith(";") ? "" : ";");
            return code + semiColon;
        }
    }
}