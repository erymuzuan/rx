using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    class GenericNameAccessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.GenericName }; }
        }

        public override bool Filter(SyntaxNode node)
        {

            return node.CSharpKind() == SyntaxKind.GenericName;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var genericSyntax = (GenericNameSyntax)node;
            //var local = this.EvaluateExpressionCode(genericSyntax.Identifier.Text);
            //var name = this.EvaluateExpressionCode(genericSyntax.Name);
            //if (string.IsNullOrWhiteSpace(local))
            //    return name;
            return string.Format("{0}.{1}", genericSyntax.Identifier.Text, "");
        }
    }
}