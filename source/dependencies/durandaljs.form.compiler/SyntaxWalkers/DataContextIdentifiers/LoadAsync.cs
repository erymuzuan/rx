using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.LoggerIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "LoadAsync")]
    public class LoadAsync : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var args = arguments.ToArray();
            return "loadAsync('" + genericTypeArguments.First() + "', " + this.EvaluateExpressionCode(args[0]) + ")";
        }

    }
}