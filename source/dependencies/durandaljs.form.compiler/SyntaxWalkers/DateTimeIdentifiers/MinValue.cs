using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DateTime", Text = "MinValue")]
    public class MinValue: IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "moment('0001-01-01')";
        }
    }
}