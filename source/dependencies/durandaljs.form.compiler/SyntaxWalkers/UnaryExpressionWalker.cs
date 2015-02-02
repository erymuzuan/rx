using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class UnaryExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.LogicalNotExpression ,
                SyntaxKind.LogicalAndExpression, 
                SyntaxKind.LogicalOrExpression
            };
            }
        }
        

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var unary = node as PrefixUnaryExpressionSyntax;
            if (null == unary) return string.Empty;

            var op = "";
            var ot = unary.OperatorToken.RawKind;
            if (ot == (int)SyntaxKind.ExclamationToken)
                op += "!";

            var code = this.EvaluateExpressionCode(unary.Operand);
            return op + code;

        }


    }
}