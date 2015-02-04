using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class BooleanMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == "System.Boolean";
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