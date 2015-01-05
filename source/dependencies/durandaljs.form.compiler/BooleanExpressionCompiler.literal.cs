using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {

        class LiteralExpressionWalker : CSharpSyntaxWalker
        {

            private string m_value = "";
            internal static string Walk(SyntaxNode node)
            {
                var kind = node.CSharpKind();
                var walker = new LiteralExpressionWalker ();
                walker.Visit(node);
                switch (kind)
                {
                        case SyntaxKind.TrueKeyword:return "true";
                        case SyntaxKind.TrueLiteralExpression:return "true";
                        case SyntaxKind.FalseKeyword:return "false";
                        case SyntaxKind.FalseLiteralExpression:return "false";
                        case SyntaxKind.NullLiteralExpression:return "null";
                        case SyntaxKind.NumericLiteralExpression:return walker.m_value;
                        case SyntaxKind.StringLiteralExpression:return walker.m_value;
                        case SyntaxKind.StringLiteralToken:return walker.m_value;
                        case SyntaxKind.NumericLiteralToken:return walker.m_value;
                }
                return string.Empty;
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                if (node.Token.Value == null)
                    m_value = "null";
                if (node.Token.Value is string)
                    m_value = string.Format("'{0}'", node.Token.Value);
                else
                    m_value = string.Format("{0}", node.Token.Value);
                base.VisitLiteralExpression(node);
            }
        }

    }
}
