using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class LambdaParameterIdentifierNamedSyntaxWalker : OdataSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IdentifierName }; }
        }

        protected override bool Filter(IParameterSymbol parameter)
        {
            return true;
        }

        protected override string Walk(IdentifierNameSyntax id, SemanticModel model)
        {
            return string.Empty;
        }
    }
}