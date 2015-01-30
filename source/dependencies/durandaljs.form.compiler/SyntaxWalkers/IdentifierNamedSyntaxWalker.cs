using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class IdentifierNamedSyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IdentifierName }; }
        }

        public override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            var local = symbol as ILocalSymbol;
            return local != null;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var syntax = node as IdentifierNameSyntax;
            if (null != syntax)
            {
                return syntax.Identifier.Text;
            }
            return string.Empty;
        }
    }
}