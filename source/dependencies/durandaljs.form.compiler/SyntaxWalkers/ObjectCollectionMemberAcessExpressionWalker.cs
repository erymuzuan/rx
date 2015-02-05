using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ObjectCollectionMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string OBJECT_COLLECTION = "ObjectCollection";
        public const string SYSTEM_COLLECTION_GENERIC = "Bespoke.Sph.Domain";
        public const string SYSTEM_COLLECTIONS_GENERIC_LIST = "Bespoke.Sph.Domain.ObjectCollection`";


        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == SYSTEM_COLLECTIONS_GENERIC_LIST;
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == OBJECT_COLLECTION &&
                   method.ContainingNamespace.ToString() == SYSTEM_COLLECTION_GENERIC;
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == OBJECT_COLLECTION)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                       prop.ContainingAssembly.Name == MSCORLIB;
            }
            return false;
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return OBJECT_COLLECTION; }
        }
    }
}