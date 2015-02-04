using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;


            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {
                if (null != prop.ContainingType && prop.ContainingType.Name == "DateTime")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                        prop.ContainingAssembly.Name == "mscorlib";
                }
                // if the property type is DateTime, then return false
                // we just interested the current type, property belong to DateTime
                return false;

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.DateTime";

            if (null == symbol.ContainingType) return false;
            if (null == symbol.ContainingNamespace) return false;
            if (null == symbol.ContainingAssembly) return false;

            // static methods and properties
            return symbol.ContainingType.Name == "DateTime" &&
                symbol.ContainingNamespace.Name == "System" &&
                symbol.ContainingAssembly.Name == "mscorlib";
        }


        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return "DateTime"; }
        }

        protected override string Walk(IdentifierNameSyntax id, SemanticModel model)
        {
            if (id.Identifier.Text == "DateTime") 
                return string.Empty;

            return base.Walk(id, model);
        }
    }
}