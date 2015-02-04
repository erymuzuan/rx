using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ListMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;


            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {   //System.Linq.Enumerable
                if (null != prop.ContainingType && prop.ContainingType.Name == "List")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                           prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "List" || named.ToString() == "System.Collections.Generic") &&
                       named.ContainingNamespace.Name == "System";

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.Collections.Generic.List`";

            if (null == symbol.ContainingType) return false;
            if (null == symbol.ContainingNamespace) return false;
            if (null == symbol.ContainingAssembly) return false;

            // static methods and propertues
            return symbol.ContainingType.Name == "List" &&
                   symbol.ContainingNamespace.ToString() == "System.Collections.Generic";
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return "List"; }
        }
    }
}