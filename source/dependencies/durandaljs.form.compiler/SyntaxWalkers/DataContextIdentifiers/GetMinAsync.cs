using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.OdataQueryCompilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DataContextIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = DataContextExpressionWalker.DATA_CONTEXT, Text = "GetMinAsync")]
    public class GetMinAsync : IdentifierCompiler
    {
        [Import]
        public OdataQueryExpressionCompiler OdataCompiler { get; set; }

        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var lambda = (SimpleLambdaExpressionSyntax) arguments.First();
            var query = OdataCompiler.CompileExpression(lambda.Body, this.GetWalker(node).SemanticModel);
            return "getMinAsync(\"" + genericTypeArguments.First() + "\", \"" + query + "\")";
        }

    }
}