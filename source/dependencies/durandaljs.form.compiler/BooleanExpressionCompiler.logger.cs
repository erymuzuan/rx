using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {

        class LoggerMemberAcessExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder("logger.");
            private string m_args;
            internal static string Walk(SyntaxNode node, string args)
            {
                if (node.CSharpKind() == SyntaxKind.InvocationExpression)
                {
                    return MethodInvocationExpressionWalker.Walk(node);
                }

                var walker = new LoggerMemberAcessExpressionWalker { m_args = args };
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                var text = node.Parent.GetText().ToString().ToLower();
                if (text.StartsWith("string.") || text.StartsWith("!string."))
                {
                    if (node.Identifier.Text == "Trim")
                        m_code.Append("trim");
                    if (node.Identifier.Text == "IsNullOrEmpty")
                        m_code.Append("isNullOrEmpty");
                    if (node.Identifier.Text == "IsNullOrWhiteSpace")
                        m_code.Append("isNullOrWhiteSpace");

                }

                m_code.AppendFormat("({0})", m_args);

                base.VisitIdentifierName(node);
            }


        }

    }
}
