using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.StringIdentifiers
{
    [Export("String", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "String", Text = "IsNullOrWhiteSpace")]
    public class StringIsNullOrWhiteSpace : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var arg = this.EvaluateExpressionCode(arguments.First());
            return string.Format("String.isNullOrWhiteSpace({0})", arg);
        }
    }
}