using System;
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
            internal static string Walk(SyntaxNode node, string args)
            {
                if (node.CSharpKind() == SyntaxKind.InvocationExpression)
                {
                    return NativeMethodInvocationExpressionWalker.Walk(node);
                }

                var walker = new DateTimeMemberAcessExpressionWalker { m_args = args };
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.Identifier.Text == "Parse")
                    m_code.AppendFormat("moment({0})", m_args);
                if (node.Identifier.Text == "ParseExact")
                    m_code.AppendFormat("moment({0})", m_args);

                base.VisitIdentifierName(node);
            }


        }

    }
}
