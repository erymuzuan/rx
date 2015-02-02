using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class BinaryExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.AddExpression,
                SyntaxKind.SubtractExpression,
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression,
                SyntaxKind.GreaterThanExpression, 
                SyntaxKind.LessThanExpression, 
                SyntaxKind.LessThanOrEqualExpression, 
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.LogicalAndExpression, 
                SyntaxKind.LogicalOrExpression,
                SyntaxKind.ModuloExpression,
                SyntaxKind.CoalesceExpression
            };
            }
        }

        public override bool Filter(SyntaxNode node)
        {
            return Kinds.Contains(node.CSharpKind());
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var kind = node.CSharpKind();
            var bes = (BinaryExpressionSyntax)node;
            var op = "";
            switch (kind)
            {
                case SyntaxKind.ModuloExpression:
                    op = "%";
                    break;
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
                case SyntaxKind.AddExpression:
                    op = "+";
                    break;
                case SyntaxKind.SubtractExpression:
                    op = "-";
                    break;
                case SyntaxKind.LogicalNotExpression:
                    op = "/* LogicalNotExpression is not implemented */";
                    break;
                default:

                    break;
            }
            if (!string.IsNullOrWhiteSpace(op))
                return this.EvaluateExpressionCode(bes.Left)
                       + " " + op + " "
                       + this.EvaluateExpressionCode(bes.Right);

            return string.Empty;
        }


  

    }
}