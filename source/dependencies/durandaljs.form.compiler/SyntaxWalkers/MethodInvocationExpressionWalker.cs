using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class MethodInvocationExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.InvocationExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return (node.CSharpKind() == SyntaxKind.InvocationExpression);
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var invocationSyntax = (InvocationExpressionSyntax)node;

            var w = this.GetWalker(invocationSyntax.Expression);
            if (null == w)
                throw new InvalidOperationException(string.Format("Cannot find walker for {0} => {1}", invocationSyntax.CSharpKind(), invocationSyntax.ToFullString()));

            var c = w.Walk(invocationSyntax.Expression, model);
            return c;

        }




    }
}