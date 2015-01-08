using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.MathSIdentifiers
{
    [Export("Math", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Math", Text = "Abs")]
    public class Abs : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = "";
            if (null != arguments)
                args = string.Join(", ", arguments.Select(this.EvaluateExpressionCode));
            return string.Format("abs({0})", args);
        }
    }
}