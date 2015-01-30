using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ParameterWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IdentifierName }; }
        }


        public override bool Filter(SymbolInfo info)
        {
            var ps = info.Symbol as IParameterSymbol;
            if (null == ps) return false;
            var reservedParameters = new[] { "item", "logger", "config", "app" };
            return !reservedParameters.Contains(ps.Name);
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = (IdentifierNameSyntax)node;
            return maes.Identifier.Text;
        }
    }
}