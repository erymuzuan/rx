using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.StringIdentifiers
{
    [Export("String", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "Length")]
    public class StringLength : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "length";
        }
    }
}