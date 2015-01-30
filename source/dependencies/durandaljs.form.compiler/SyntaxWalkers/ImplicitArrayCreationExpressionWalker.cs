using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ImplicitArrayCreationExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ArrayCreationExpression }; }
        }

    
        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var rs = (ImplicitArrayCreationExpressionSyntax)node;
            var codes = rs.Initializer.Expressions.Select(x => this.GetStatementCode(model, x));
            var code = string.Join(", ", codes);

            return "[" + code + "]";
        }
    }
}