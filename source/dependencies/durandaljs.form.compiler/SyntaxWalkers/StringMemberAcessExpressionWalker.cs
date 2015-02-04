using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class StringMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.String";

            if (null == info.Symbol.ContainingType) return false;
            if (null == info.Symbol.ContainingNamespace) return false;
            if (null == info.Symbol.ContainingAssembly) return false;

            return info.Symbol.ContainingType.Name == "string" ||
                info.Symbol.ContainingType.Name == "String";
        }

        protected override string InferredTypeName
        {
            get { return "String"; }
        }

        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxKind.PredefinedType
            };
            }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var code = node.ToFullString();
            if (code.ToLowerInvariant() == "string.empty")
                return "''";

            return base.Walk(node, model);
        }
        
    }
}