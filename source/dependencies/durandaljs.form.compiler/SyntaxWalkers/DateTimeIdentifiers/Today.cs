using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DateTime", Text = "Today")]
    public class Today: IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return "moment().startOf('day')";
        }
    }
}