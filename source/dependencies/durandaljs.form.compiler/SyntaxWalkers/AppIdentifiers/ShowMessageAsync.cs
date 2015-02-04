using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.AppIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = AppExpressionWalker.APPLICATION_HELPER, Text = "ShowMessageAsync")]
    public class ShowMessageAsync : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            return string.Format("showMessage({0}, {1})", this.EvaluateExpressionCode(args[0]), this.EvaluateExpressionCode(args[1]));
        }
    }
}