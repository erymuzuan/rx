using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers.DateTimeIdentifiers
{
    [Export("DateTime", typeof(IdentifierCompiler))]
    [IdentifierCompilerMetadata(TypeName = "DateTime", Text = "ParseExact")]
    public class DateTimeParseExact : IdentifierCompiler
    {
        public override string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> argumentList)
        {
            var arguments = argumentList.Select(this.EvaluateExpressionCode).ToList();

            var formatArgument = (ArgumentSyntax)node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                       .Single()
                       .ChildNodes()
                       .ToArray()[1];
            var literalFormat = formatArgument.Expression as LiteralExpressionSyntax;
            if (null != literalFormat && !string.IsNullOrWhiteSpace(literalFormat.Token.ValueText))
            {
                var momentFormat = literalFormat.Token.ValueText.Replace("d", "D")
                    .Replace("y", "Y");
                return string.Format("moment({0}, {1})", arguments[0], arguments[1].Replace(literalFormat.Token.ValueText, momentFormat));
            }

            return string.Format("moment({0}, {1})", arguments[0], arguments[1]);

        }
    }
}