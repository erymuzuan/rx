using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.LoggerIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "LoadOneAsync")]
    public class LoadOneAsync : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var args = arguments.ToArray();
            return "loadOneAsync('" + genericTypeArguments.First() + "', " + this.EvaluateExpressionCode(args[0]) + ")";
        }

    }
}