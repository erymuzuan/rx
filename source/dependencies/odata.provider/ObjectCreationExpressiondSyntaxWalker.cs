using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class ObjectCreationExpressiondSyntaxWalker : OdataSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ObjectCreationExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            Console.WriteLine(node.CSharpKind());
            return node.CSharpKind() == SyntaxKind.ObjectCreationExpression;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var creation = (ObjectCreationExpressionSyntax) node;
            var v1 = this.EvaluateExpressionCode(creation.Type);
            var args = creation.ArgumentList.Arguments.Select(x => this.EvaluateExpressionCode(x.Expression));

            return v1 + string.Join(", ", args);
        }
    }
}