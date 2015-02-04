using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class Int32SyntaxWalker : CustomObjectSyntaxWalker
    {
        public const string INT32 = "Int32";
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == INT32 &&
                method.ContainingNamespace.Name == SYSTEM &&
                method.ContainingAssembly.Name == MSCORLIB;
        }

        protected override bool Filter(IFieldSymbol field)
        {
            return field.ContainingType.Name == INT32 &&
                field.ContainingNamespace.Name == SYSTEM &&
                field.ContainingAssembly.Name == MSCORLIB;
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == INT32)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                    prop.ContainingAssembly.Name == MSCORLIB;
            }

            return false;
        }

        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == SYSTEM + "." + INT32;
        }

        protected override string InferredTypeName
        {
            get { return INT32; }
        }

    }
}