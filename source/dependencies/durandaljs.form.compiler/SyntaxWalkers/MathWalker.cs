using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class MathWalker : CustomObjectSyntaxWalker
    {
        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == "System.Math";
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == "Math" && method.ContainingNamespace.Name == "System";
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override string InferredTypeName
        {
            get { return "Math"; }
        }
    }
}