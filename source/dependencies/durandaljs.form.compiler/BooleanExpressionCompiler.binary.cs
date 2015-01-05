using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {
        class BinaryExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new BinaryExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                var operatorToken = node.OperatorToken.Text;
                var ot = node.OperatorToken.RawKind;
                if (ot == (int)SyntaxKind.EqualsEqualsToken)
                    operatorToken = "===";
                if (ot == (int)SyntaxKind.ExclamationEqualsToken)
                    operatorToken = "!==";


                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Left));
                m_code.AppendFormat(" {0} ", operatorToken);
                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Right));
                base.VisitBinaryExpression(node);
            }
        }
    }
}