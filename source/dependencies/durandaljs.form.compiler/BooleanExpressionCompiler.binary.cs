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


                // TODO: refactor into another visitor
                m_code.Append(MethodInvocationExpressionWalker.Walk(node.Left));
                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Left));
                m_code.Append(DateTimeMemberAcessExpressionWalker.Walk(node.Left));
                m_code.Append(LiteralExpressionWalker.Walk(node.Left));
                m_code.Append(ConfigMemberAcessExpressionWalker.Walk(node.Left));
                m_code.Append(StringMemberAcessExpressionWalker.Walk(node.Left,null));


                m_code.AppendFormat(" {0} ", operatorToken);

                // TODO: refactor into another visitor
                var right = node.Right;
                m_code.Append(ItemMemberAccessExpressionWalker.Walk(right));
                m_code.Append(MethodInvocationExpressionWalker.Walk(right));
                m_code.Append(DateTimeMemberAcessExpressionWalker.Walk(right));
                m_code.Append(LiteralExpressionWalker.Walk(right));
                m_code.Append(ConfigMemberAcessExpressionWalker.Walk(right));
                m_code.Append(StringMemberAcessExpressionWalker.Walk(right,null));

                base.VisitBinaryExpression(node);
            }
        }
    }
}