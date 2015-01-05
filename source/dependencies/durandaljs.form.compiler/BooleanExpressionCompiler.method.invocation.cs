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
                var ies = node as InvocationExpressionSyntax;
                if (null == ies) return string.Empty;

                var walker = new MethodInvocationExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }


            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var parent = node.Expression.GetText().ToString();

                m_args.Clear();
                m_args.AddRange(node.ArgumentList.Arguments
                    .Select(this.GetArgumentSyntax)
                    .Where(x => !string.IsNullOrWhiteSpace(x)))
                   ;

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

            private string GetArgumentSyntax(ArgumentSyntax arg)
            {
                var les = arg.Expression as LiteralExpressionSyntax;
                if (null != les)
                {
                    if (null == les.Token.Value)
                    {
                        return "null";
                    }
                    if (les.Token.Value is string)
                    {
                        return (string.Format("'{0}'", les.Token.Value));
                    }
                    return (string.Format("{0}", les.Token.Value));

                }

                var maes = arg.Expression as MemberAccessExpressionSyntax;
                if (null != maes)
                {
                    var value = ItemMemberAccessExpressionWalker.Walk(arg.Expression);
                    if (!string.IsNullOrWhiteSpace(value))
                        return (value);
                    value = ConfigMemberAcessExpressionWalker.Walk(arg.Expression);
                    if (!string.IsNullOrWhiteSpace(value))
                        return (value);
                    value = DateTimeMemberAcessExpressionWalker.Walk(arg.Expression);
                    if (!string.IsNullOrWhiteSpace(value))
                        return (value);

                }

                var bes = arg.Expression as BinaryExpressionSyntax;
                if (null != bes)
                {
                    return BinaryExpressionWalker.Walk(arg.Expression);
                }

                var ies = arg.Expression as InvocationExpressionSyntax;
                if (null != ies)
                {
                    var value = Walk(arg.Expression);
                    return (value);
                }

                return null;
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
                    m_code.AppendFormat(".indexOf({0}) > -1", string.Join(", ", m_args));
                }
                base.VisitIdentifierName(node);
            }
        }
    }
}