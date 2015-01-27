using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.EnumerableIdentifiers
{
    [Export("Enumerable", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "Enumerable", Text = "Count")]
    public class Count : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {

            var args = arguments.ToArray();
            if (args.Length == 0)
                return string.Format("length");

            // TODO : write extension method called "count" to use underscorejs or whatever in the array prototype
            // e.g : http://underscorejs.org/#countBy
            return string.Format("count({0})", this.EvaluateExpressionCode(args[0]));
        }
    }
}