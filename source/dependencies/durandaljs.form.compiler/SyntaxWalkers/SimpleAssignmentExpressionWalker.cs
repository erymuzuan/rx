using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class SimpleAssignmentExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleAssignmentExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.SimpleAssignmentExpression;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var assignment = (AssignmentExpressionSyntax) node;
            return this.EvaluateExpressionCode(assignment.Left) + " = "
                + this.EvaluateExpressionCode(assignment.Right);
        }
    }
}