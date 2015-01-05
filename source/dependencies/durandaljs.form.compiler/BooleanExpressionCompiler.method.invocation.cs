using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {

        class MethodInvocationExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            private readonly List<string> m_args = new List<string>();

            internal static string Walk(SyntaxNode node)
            {
                var walker = new MethodInvocationExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }
            private static string GetArgs(SyntaxNode node)
            {
                var walker = new MethodInvocationExpressionWalker();
                walker.Visit(node);
                return string.Join(", " ,walker.m_args);
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var parent = node.Expression.GetText().ToString();
                var itemMemberArgs = node.ArgumentList.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
                    .Select(ItemMemberAccessExpressionWalker.Walk);
                var literalArgs = node.ArgumentList.DescendantNodes().OfType<LiteralExpressionSyntax>()
                    .Select(GetArgs);

                m_args.AddRange(itemMemberArgs);
                m_args.AddRange(literalArgs);
                m_args.RemoveAll(string.IsNullOrWhiteSpace);

                var args = string.Join(", ", m_args);

                if (parent.StartsWith("DateTime"))
                {
                    m_code.Append(DateTimeMemberAcessExpressionWalker.Walk(node.Expression, args));
                }

                if (parent.StartsWith("logger"))
                {
                    m_code.Append(LoggerMemberAcessExpressionWalker.Walk(node.Expression, args));
                }
                if (parent.StartsWith("config"))
                {
                    m_code.Append(ConfigMemberAcessExpressionWalker.Walk(node.Expression));
                }
                if (parent.StartsWith("string") || parent.StartsWith("!string"))
                {
                    m_code.Append(StringMemberAcessExpressionWalker.Walk(node.Expression, args));
                }

                base.VisitInvocationExpression(node);
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                var value = node.Token.Value;
                if (value is string)
                {
                    m_args.Add(string.Format("'{0}'", value));
                }
                else
                {
                    m_args.Add(node.Token.ValueText == "null" ? "null" : string.Format("{0}", value));
                }
                base.VisitLiteralExpression(node);
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                // for array .ContainsMethod
                if (node.Identifier.Text == "Contains")
                {
                    m_code.AppendFormat(".indexOf({0}) > -1", string.Join(", ",m_args));
                }
                base.VisitIdentifierName(node);
            }
        }
    }
}