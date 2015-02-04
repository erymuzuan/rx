using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ListMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string LIST = "List";
        public const string SYSTEM_COLLECTION_GENERIC = "System.Collections.Generic";
        public const string SYSTEM_COLLECTIONS_GENERIC_LIST = "System.Collections.Generic.List`";


        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == SYSTEM_COLLECTIONS_GENERIC_LIST;
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == LIST &&
                   method.ContainingNamespace.ToString() == SYSTEM_COLLECTION_GENERIC;
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == LIST)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                       prop.ContainingAssembly.Name == MSCORLIB;
            }

            var named = prop.Type;
            return (named.Name == LIST || named.ToString() == SYSTEM_COLLECTION_GENERIC) &&
                   named.ContainingNamespace.Name == SYSTEM;
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return LIST; }
        }
    }
}