using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DateTime", Text = "AddMilliseconds")]
    public class AddMillis : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            return "add(" + this.EvaluateExpressionCode(args[0]) + ", 'ms')";
        }
    }
}