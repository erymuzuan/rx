using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {

        class NativeMethodInvocationExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            private string m_args;

            internal static string Walk(SyntaxNode node)
            {
                var walker = new NativeMethodInvocationExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var parent = node.Expression.GetText().ToString();
                var itemMember = node.ArgumentList.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
                    .SingleOrDefault();
                var literal = node.ArgumentList.DescendantNodes().OfType<LiteralExpressionSyntax>()
                    .SingleOrDefault();
            

                if (null != itemMember)
                {
                    m_args = ItemMemberAccessExpressionWalker.Walk(itemMember);
                }

                if (null != literal)
                {
                    var w = new NativeMethodInvocationExpressionWalker();
                    w.Visit(literal);
                    m_args = w.m_args;
                }

                if (parent.StartsWith("DateTime"))
                {
                    m_code.Append(DateTimeMemberAcessExpressionWalker.Walk(node.Expression, m_args));
                }
                if (parent.StartsWith("logger"))
                {
                    m_code.Append(LoggerMemberAcessExpressionWalker.Walk(node.Expression, m_args));
                }
                if (parent.StartsWith("config"))
                {
                    m_code.Append(ConfigMemberAcessExpressionWalker.Walk(node.Expression));
                }
                if (parent.StartsWith("string") || parent.StartsWith("!string"))
                {
                    m_code.Append(StringMemberAcessExpressionWalker.Walk(node.Expression,m_args));
                }

                base.VisitInvocationExpression(node);
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                var value = node.Token.Value;
                if (value is string)
                {
                    m_args = string.Format("'{0}'", value);
                }
                else
                {
                    m_args = string.Format("{0}", value);
                    if (node.Token.ValueText == "null")
                        m_args = "null";
                }
                base.VisitLiteralExpression(node);
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                // for array .ContainsMethod
                if (node.Identifier.Text == "Contains")
                {
                    m_code.AppendFormat(".indexOf({0}) > -1", m_args);
                }
                base.VisitIdentifierName(node);
            }
        }
    }
}