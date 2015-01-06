using System;
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
                SyntaxKind.LogicalOrExpression
            };
            }
        }

        public override string Walk(SyntaxNode node)
        {
            var kind = node.CSharpKind();
            var bes = (BinaryExpressionSyntax)node;
            switch (kind)
            {
                case SyntaxKind.LogicalAndExpression:
                    return this.EvaluateExpressionCode(bes.Left)
                           + " && "
                           + this.EvaluateExpressionCode(bes.Right);
                case SyntaxKind.LogicalOrExpression:
                    return this.EvaluateExpressionCode(bes.Left)
                           + " || "
                           + this.EvaluateExpressionCode(bes.Right);
                case SyntaxKind.LogicalNotExpression:
                    throw new Exception("Not implemented for LogicalNotExpression :" + node);
            }

            return base.Walk(node);
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