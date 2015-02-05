using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.OdataQueryCompilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.LoggerIdentifiers
{
    [Export(typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DataContext", Text = "SearchAsync")]
    public class SearchAsync : IdentifierCompiler
    {
        [Import]
        public OdataQueryExpressionCompiler OdataCompiler { get; set; }

        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments)
        {

            return "searchAsync(\"" + genericTypeArguments.First() + "\", JSON.stringify(" + arguments.First() + "))";
        }

    }
}