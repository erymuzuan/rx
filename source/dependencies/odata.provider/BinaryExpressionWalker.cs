using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export(typeof(OdataSyntaxWalker))]
    class BinaryExpressionWalker : OdataSyntaxWalker
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
            string op;
            switch (kind)
            {
                case SyntaxKind.LogicalAndExpression:
                    op = "and";
                    break;
                case SyntaxKind.LogicalOrExpression:
                    op = "or";
                    break;
                case SyntaxKind.EqualsExpression:
                    op = "eq";
                    break;
                case SyntaxKind.NotEqualsExpression:
                    op = "ne";
                    break;
                case SyntaxKind.GreaterThanExpression:
                    op = "gt";
                    break;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    op = "ge";
                    break;
                case SyntaxKind.LessThanExpression:
                    op = "lt";
                    break;
                case SyntaxKind.LessThanOrEqualExpression:
                    op = "le";
                    break;
                default: throw new Exception(kind + " not implemented for Odata");
            }
            if (!string.IsNullOrWhiteSpace(op))
                return this.EvaluateExpressionCode(bes.Left)
                       + " " + op + " "
                       + this.EvaluateExpressionCode(bes.Right);

            return string.Empty;
        }




    }
}