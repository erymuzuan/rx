using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class BinaryExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return null; }
        }

        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression,
                SyntaxKind.GreaterThanExpression, 
                SyntaxKind.LessThanExpression, 
                SyntaxKind.LessThanOrEqualExpression, 
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LogicalAndExpression, 
                SyntaxKind.LogicalOrExpression,
                SyntaxKind.CoalesceExpression
            };
            }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var kind = node.CSharpKind();
            var bes = (BinaryExpressionSyntax)node;
            var op = "";
            switch (kind)
            {
                case SyntaxKind.CoalesceExpression:
                    op = "||";
                    break;
                case SyntaxKind.LogicalAndExpression:
                    op = "&&";
                    break;
                case SyntaxKind.LogicalOrExpression:
                    op = "||";
                    break;
                case SyntaxKind.EqualsExpression:
                    op = "===";
                    break;
                case SyntaxKind.NotEqualsExpression:
                    op = "!==";
                    break;
                case SyntaxKind.GreaterThanExpression:
                    op = ">";
                    break;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    op = ">=";
                    break;
                case SyntaxKind.LessThanExpression:
                    op = "<";
                    break;
                case SyntaxKind.LessThanOrEqualExpression:
                    op = "<=";
                    break;
                case SyntaxKind.LogicalNotExpression:
                    op = "/* LogicalNotExpression is not implemented */";
                    break;
                default:

                    break;
            }
            if(!string.IsNullOrWhiteSpace(op))

                return this.EvaluateExpressionCode(bes.Left)
                       + " " + op + " "
                       + this.EvaluateExpressionCode(bes.Right);

            return base.Walk(node, model);
        }


        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var operatorToken = node.OperatorToken.Text;
            var ot = node.OperatorToken.RawKind;
            if (ot == (int)SyntaxKind.EqualsEqualsToken)
                operatorToken = "===";
            if (ot == (int)SyntaxKind.ExclamationEqualsToken)
                operatorToken = "!==";

            var left = this.EvaluateExpressionCode(node.Left);
            var right = this.EvaluateExpressionCode(node.Right);
            this.Code.AppendFormat("{0} {1} {2}", left, operatorToken, right);

            base.VisitBinaryExpression(node);
        }
    }
}