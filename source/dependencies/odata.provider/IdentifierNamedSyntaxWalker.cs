using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class IdentifierNamedSyntaxWalker : OdataSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IdentifierName }; }
        }

        protected override bool Filter(IPropertySymbol prop)
        {
            return true;
        }

        protected override string Walk(IdentifierNameSyntax id, SemanticModel model)
        {
            return id.Identifier.Text;
        }
    }
}