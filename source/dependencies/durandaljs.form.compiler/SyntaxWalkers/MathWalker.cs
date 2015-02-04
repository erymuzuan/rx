using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class MathWalker : CustomObjectSyntaxWalker
    {
        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.Math";

            var ms = info.Symbol as IMethodSymbol;
            if (null != ms)
                return ms.ContainingType.Name == "Math" && ms.ContainingNamespace.Name == "System";

            if (null == info.Symbol.ContainingType) return false;
            if (null == info.Symbol.ContainingNamespace) return false;
            if (null == info.Symbol.ContainingAssembly) return false;

            return info.Symbol.ContainingType.ToString() == "Math";
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return "Math"; }
        }
    }
}