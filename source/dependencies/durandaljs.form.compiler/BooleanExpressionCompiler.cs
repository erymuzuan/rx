using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(BooleanExpressionCompiler))]
    public class BooleanExpressionCompiler
    {
        public string Compile(string expression, EntityDefinition entity)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return "true";

            var file = new StringBuilder("public bool ");
            file.AppendLinf("Evaluate({0} item)  ", entity.Name);
            file.AppendLine("{");
            file.AppendLinf(" return {0};", expression);
            file.AppendLine("}");


            var tree = CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var statement = root.DescendantNodes().OfType<ReturnStatementSyntax>()
                .Single()
                .Expression;

            return CompileExpression(statement);
        }

        private string CompileExpression(SyntaxNode statement)
        {
            if (statement is LiteralExpressionSyntax)
            {
                switch (statement.CSharpKind())
                {
                    case SyntaxKind.TrueLiteralExpression:
                        return "true";
                    case SyntaxKind.FalseLiteralExpression:
                        return "false";
                    default: throw new NotSupportedException("\""+ statement.GetText()+"\" expression is not supported");
                }
            }

            var parenthesiz = statement as ParenthesizedExpressionSyntax;
            if (null != parenthesiz)
            {
                return "(" + this.CompileExpression(parenthesiz.Expression) + ")";
            }
            var bes = statement as BinaryExpressionSyntax;
            if (bes != null)
            {
                switch (bes.RawKind)
                {
                    case (int)SyntaxKind.LogicalAndExpression:
                        return this.CompileExpression(bes.Left)
                               + " && "
                               + this.CompileExpression(bes.Right);
                    case (int)SyntaxKind.LogicalOrExpression:
                        return this.CompileExpression(bes.Left)
                               + " || "
                               + this.CompileExpression(bes.Right);
                    case (int)SyntaxKind.LogicalNotExpression:
                        throw new Exception("Not implemented for LogicalNotExpression :" + statement.GetText());
                }
            }

            if (statement is PrefixUnaryExpressionSyntax)
            {
                return UnaryExpressionWalker.Walk(statement);
            }


            var code = BinaryExpressionWalker.Walk(statement);
            if (string.IsNullOrWhiteSpace(code))
                code = ItemMemberAccessExpressionWalker.Walk(statement);
            return code;
        }

        class UnaryExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new UnaryExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
            {
                var ot = node.OperatorToken.RawKind;
                if (ot == (int)SyntaxKind.ExclamationToken)
                    m_code.Append("!");

                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Operand));
                base.VisitPrefixUnaryExpression(node);
            }


        }

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


                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Left));
                m_code.AppendFormat(" {0} ", operatorToken);
                m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.Right));
                base.VisitBinaryExpression(node);
            }
        }

        class NativeMethodInvocationExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new NativeMethodInvocationExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (
                    node.Expression.GetText()
                        .ToString()
                        .ToLowerInvariant()
                        .StartsWith("string.IsNullOr".ToLowerInvariant()))
                {
                    m_code.Append("!");
                    m_code.Append(ItemMemberAccessExpressionWalker.Walk(node.ArgumentList.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
                        .Single()));
                }
                base.VisitInvocationExpression(node);
            }
        }

        class ItemMemberAccessExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                if (node.CSharpKind() == SyntaxKind.InvocationExpression)
                {
                    return NativeMethodInvocationExpressionWalker.Walk(node);
                }

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