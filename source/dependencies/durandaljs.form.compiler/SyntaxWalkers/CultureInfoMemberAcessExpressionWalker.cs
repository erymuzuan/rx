using System.ComponentModel.Composition;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class CultureInfoMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == typeof(CultureInfo).FullName;
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