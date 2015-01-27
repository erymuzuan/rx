using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.EnumerableIdentifiers
{
    [Export("Enumerable", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Enumerable", Text = "Where")]
    public class Where : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            return string.Format("filter({0})", this.EvaluateExpressionCode(args[0]));
        }
    }
}