using System.ComponentModel.Composition;
using System.Linq;
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

    
        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var rs = (ReturnStatementSyntax)node;
            var code = this.Walkers
                .Where(x => x.Filter(rs.Expression))
                .Select(x => x.Walk(rs.Expression, model))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            return "return " + code + ";";
        }
    }
}