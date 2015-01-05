using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {
        class StringMemberAcessExpressionWalker : CSharpSyntaxWalker
        {
            private string m_code = "";
            private string m_args;
            internal static string Walk(SyntaxNode node, string args = null)
            {
                if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression) return string.Empty;
                var pts = ((MemberAccessExpressionSyntax)node).Expression as PredefinedTypeSyntax;
                if (null == pts) return string.Empty;
                if (pts.ToString().ToLower() != "string")
                    return string.Empty;
                var walker = new StringMemberAcessExpressionWalker {m_args = args};
                walker.Visit(node);
                return walker.m_code;
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                var text = node.Parent.GetText().ToString().ToLower();
                if (text.StartsWith("string.") || text.StartsWith("!string."))
                {
                    if (node.Identifier.Text == "Trim")
                        m_code = string.Format("String.trim({0})", m_args);
                    if (node.Identifier.Text == "IsNullOrEmpty")
                        m_code = string.Format("String.isNullOrEmpty({0})", m_args);
                    if (node.Identifier.Text == "IsNullOrWhiteSpace")
                        m_code = string.Format("String.isNullOrWhiteSpace({0})", m_args);
                    if (node.Identifier.Text == "Empty")
                        m_code = "''";

                }
                

                base.VisitIdentifierName(node);
            }


        }

    }
}