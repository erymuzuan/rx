using System.Collections.Generic;
using System.ComponentModel.Composition;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NAMESPACE
{
    [Export("String", typeof (IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "ToLower")]
    public class ToLower : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "toLocaleLowerCase()";
        }
    }
}