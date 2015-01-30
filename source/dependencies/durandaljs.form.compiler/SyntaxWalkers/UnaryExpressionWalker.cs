using System.ComponentModel.Composition;
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



        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var op = "";
            var ot = node.OperatorToken.RawKind;
            if (ot == (int)SyntaxKind.ExclamationToken)
                op += "!";

            var code = this.EvaluateExpressionCode(node.Operand);
            this.Code.Append(op + code);


            base.VisitPrefixUnaryExpression(node);
        }


    }
}