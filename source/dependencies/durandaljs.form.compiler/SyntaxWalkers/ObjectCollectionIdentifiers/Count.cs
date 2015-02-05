using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.ObjectCollectionIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "ObjectCollection", Text = "Count")]
    public class Count : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            var args = arguments.ToArray();
            if (args.Length == 0)
                return string.Format("length");

            return string.Format("count({0})", this.EvaluateExpressionCode(args[0]));
        }
    }
}