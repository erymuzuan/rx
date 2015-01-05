using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {


        class DateTimeMemberAcessExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            private string m_args;
            internal static string Walk(SyntaxNode node, string args = null)
            {
                if (node.CSharpKind() == SyntaxKind.InvocationExpression)
                {
                    return MethodInvocationExpressionWalker.Walk(node);
                }
                if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression) return string.Empty;
                var identifier = ((MemberAccessExpressionSyntax)node).Expression as IdentifierNameSyntax;
                if (null == identifier) return string.Empty;
                if (identifier.Identifier.Text != "DateTime")
                    return string.Empty;

                var walker = new DateTimeMemberAcessExpressionWalker { m_args = args };
                walker.Visit(node);
                return walker.m_code.ToString();
            }


            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.Identifier.Text == "Parse")
                    m_code.AppendFormat("moment({0})", m_args);
                if (node.Identifier.Text == "ParseExact")
                    m_code.AppendFormat("moment({0})", m_args.Replace("d", "D").Replace("y", "Y"));
                if (node.Identifier.Text == "Now")
                    m_code.Append("moment()");

                base.VisitIdentifierName(node);
            }


        }

    }
}
