using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "DateTime" }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var arguments = this.GetArguments(node).Select(this.EvaluateExpressionCode).ToList();
            var args = string.Join(", ", arguments);
            if (node.Identifier.Text == "Parse")
                this.Code.AppendFormat("moment({0})", args);
            if (node.Identifier.Text == "ParseExact")
            {
                var formatArgument = (ArgumentSyntax)node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                    .Single()
                    .ChildNodes()
                    .ToArray()[1];
                var literalFormat = formatArgument.Expression as LiteralExpressionSyntax;
                if (null != literalFormat && !string.IsNullOrWhiteSpace(literalFormat.Token.ValueText))
                {
                    var momentFormat = literalFormat.Token.ValueText.Replace("d", "D")
                        .Replace("y", "Y");
                    this.Code.AppendFormat("moment({0}, {1})", arguments[0], arguments[1].Replace(literalFormat.Token.ValueText, momentFormat));
                }
                else
                {
                    this.Code.AppendFormat("moment({0}, {1})", arguments[0], arguments[1]);

                }


            }
            if (node.Identifier.Text == "Now")
                this.Code.Append("moment()");
            if (node.Identifier.Text == "Today")
                this.Code.Append("moment().startOf('day')");

            base.VisitIdentifierName(node);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            Console.WriteLine(node);
            var s = node.Token.Value as string;
            if (s != null && s == "dd/MM/yyyy")
                Console.WriteLine(@"DD/MM/YYYY");
            base.VisitLiteralExpression(node);
        }

    }
}