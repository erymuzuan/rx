using System;
using System.ComponentModel.Composition;
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
        public string Compile(string expression, string entity)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return "true";

            var file = new StringBuilder("public bool ");
            file.AppendLinf("Evaluate({0} item)  ", entity);
            file.AppendLine("{");
            file.AppendLinf(" return {0};", expression);
            file.AppendLine("}");

            Console.WriteLine(file);
            var tree = CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var code = BooleanExpressionWalker.Walk(root);
            if (string.IsNullOrWhiteSpace(code))
                code = LeftRightExpressionWalker.Walk(root);
            return code;
        }

        class BooleanExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new BooleanExpressionWalker();
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


                m_code.Append(LeftRightExpressionWalker.Walk(node.Left));
                m_code.AppendFormat(" {0} ", operatorToken);
                m_code.Append(LeftRightExpressionWalker.Walk(node.Right));
                base.VisitBinaryExpression(node);
            }


        }
        class LeftRightExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {
                var walker = new LeftRightExpressionWalker();
                walker.Visit(node);
                return walker.m_code.ToString();
            }



            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.Parent.GetText().ToString().StartsWith("item."))
                    m_code.Append(node.Identifier.Text +"()");
                if (node.Identifier.Text == "item")
                    m_code.Append(".");
                base.VisitIdentifierName(node);
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
                    if (node.Token.ValueText == "null") ;
                    m_code.Append("null");
                }
                base.VisitLiteralExpression(node);
            }
        }
    }


}