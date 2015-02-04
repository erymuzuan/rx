using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class EnumerableMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string ENUMERABLE = "Enumerable";
        public const string SYSTEM_LINQ_ENUMERABLE = "System.Linq.Enumerable";
        public const string SYSTEM_LINQ = "System.Linq";

        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == SYSTEM_LINQ_ENUMERABLE;
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == ENUMERABLE
                   && method.ContainingNamespace.ToString() == SYSTEM_LINQ;
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == ENUMERABLE)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                       prop.ContainingAssembly.Name == MSCORLIB;
            }
            return false;

        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.IdentifierName, SyntaxKind.InvocationExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return ENUMERABLE; }
        }
    }
}