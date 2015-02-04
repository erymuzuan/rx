using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.StringIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "Format")]
    public class Format : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            var code = new StringBuilder("String.format(");
            var formatArguments = from a in args
                                  select this.EvaluateExpressionCode(a);
            code.Append(string.Join(", ", formatArguments));
            code.Append(")");
            return code.ToString();
        }
    }
}