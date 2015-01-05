using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {

        class ItemMemberAccessExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {

                if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression) return string.Empty;
                var identifier = ((MemberAccessExpressionSyntax)node).Expression as IdentifierNameSyntax;
                if (null == identifier) return string.Empty;
                if (identifier.Identifier.Text != "item")
                    return string.Empty;


                var walker = new ItemMemberAccessExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.Identifier.Text == "item")
                    m_code.Append("$data");
                if (node.Parent.GetText().ToString().StartsWith("item.") && node.Identifier.Text != "item")
                    m_code.Append(node.Identifier.Text + "()");
                if (node.Identifier.Text == "item")
                    m_code.Append(".");
                base.VisitIdentifierName(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                var text = node.GetText().ToString();
                if (text == "string.Empty" || text == "String.Empty")
                    m_code.Append("''");
                base.VisitMemberAccessExpression(node);
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                var value = node.Token.Value;
                if (value is string)
                {
                    m_code.AppendFormat("'{0}'", value);
                }
                else
                {
                    m_code.Append(value);
                    if (node.Token.ValueText == "null")
                        m_code.Append("null");
                }
                base.VisitLiteralExpression(node);
            }
        }
    }
}