using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.MathIdentifiers
{
    [Export( typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Math", Text = "Math")]
    public class Math : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "Math";
        }
    }
}