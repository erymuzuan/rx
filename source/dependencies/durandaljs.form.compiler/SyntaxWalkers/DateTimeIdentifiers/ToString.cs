using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DateTime", Text = "ToString")]
    public class ToString : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = "";
            if (null != arguments)
                args = string.Join(", ", arguments.Select(this.EvaluateExpressionCode));
            return "format(" + args + ")";
        }
    }
}