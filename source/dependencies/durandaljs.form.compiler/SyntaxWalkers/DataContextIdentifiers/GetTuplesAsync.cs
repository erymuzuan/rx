using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.OdataQueryCompilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DataContextIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "GetTuplesAsync")]
    public class GetTuplesAsync : IdentifierCompiler
    {
        [Import]
        public OdataQueryExpressionCompiler OdataCompiler { get; set; }

        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var lambda = (SimpleLambdaExpressionSyntax) arguments.First();
            var query = OdataCompiler.CompileExpression(lambda.Body, this.GetWalker(node).SemanticModel);
            return "getTuplesAsync(\"" + genericTypeArguments.First() + "\", \"" + query + "\")";
        }

    }
}