using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.OdataQueryCompilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DataContextIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "GetListAsync")]
    public class GetListAsync : IdentifierCompiler
    {
        [Import]
        public OdataQueryExpressionCompiler OdataCompiler { get; set; }

        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {
            var genericNames = genericTypeArguments.ToArray();
            var args2 = arguments.ToArray();
            var lambda = (SimpleLambdaExpressionSyntax) args2[1];
            var query = OdataCompiler.CompileExpression(lambda.Body, this.GetWalker(node).SemanticModel);
            return "getListAsync(\"" + genericNames[0] + "\", \"" + this.EvaluateExpressionCode(args2.First()).Replace("'","") + "\", \"" + query + "\")";
        }

    }
}