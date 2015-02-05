using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.OdataQueryCompilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.LoggerIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "GetScalarAsync")]
    public class GetScalarAsync : IdentifierCompiler
    {
        [Import]
        public OdataQueryExpressionCompiler OdataCompiler { get; set; }

        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var args = arguments.ToArray();
            var selector = (SimpleLambdaExpressionSyntax) args.First();
            var field = ((MemberAccessExpressionSyntax) selector.Body).Name.Identifier.Text;


            var lambda = (SimpleLambdaExpressionSyntax) args.Last();
            var query = OdataCompiler.CompileExpression(lambda.Body, this.GetWalker(node).SemanticModel);
            
            return "getScalarAsync(\"" + genericTypeArguments.First() + "\", \"" + field + "\", \"" + query + "\")";
        }

    }
}