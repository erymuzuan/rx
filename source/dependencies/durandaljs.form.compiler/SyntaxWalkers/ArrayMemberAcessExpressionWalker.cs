using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ArrayMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string InferredTypeName
        {
            get { return "Array"; }
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            return prop.ContainingType.Name == "Array"
                && prop.ContainingNamespace.Name == SYSTEM;
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




    }
}