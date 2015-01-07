using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            return info.Symbol.ContainingType.ToString() == "DateTime";
        }
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
            // NOTE : calling this.Evaluate will reset this.Code
            var code = this.Code.ToString();
            var arguments = this.GetArguments(node).Select(this.EvaluateExpressionCode).ToList();
            var args = string.Join(", ", arguments);
            this.Code.Append(code);


            var text = node.Identifier.Text;
            if (text == "Parse")
                this.Code.AppendFormat("moment({0})", args);
            if (text == "ParseExact")
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

            switch (text)
            {
                // instance properties
                case "Date":
                case "Day":
                case "DayOfWeek":
                case "DayOfYear":
                case "Hour":
                case "Kind":
                case "Millisecond":
                case "Minute":
                case "Month":
                case "Second":
                case "Ticks":
                case "TimeOfDay":
                case "Year":
                    break;

                // instance methods
                case "Add":
                case "AddDays":
                case "AddHours":
                case "AddMilliseconds":
                case "AddMinutes":
                case "AddMonths":
                case "AddSeconds":
                case "AddTicks":
                case "AddYears":
                case "CompareTo":
                case "Equals":
                case "IsDaylightSavingTime":
                case "ToBinary":
                case "GetHashCode":
                case "Subtract":
                case "ToOADate":
                case "ToFileTime":
                case "ToFileTimeUtc":
                case "ToLocalTime":
                case "ToLongDateString":
                case "ToLongTimeString":
                case "ToShortDateString":
                case "ToShortTimeString":
                case "ToString":
                case "ToUniversalTime":
                case "GetDateTimeFormats":
                case "GetTypeCode":
                    break;

                // static properties
                case "Now":
                    this.Code.Append("moment()");
                    break;
                case "UtcNow": break;
                case "Today":
                    this.Code.Append("moment().startOf('day')");
                    break;

                // static methods
                case "Compare":
                case "DaysInMonth":
                case "FromBinary":
                case "FromFileTime":
                case "FromFileTimeUtc":
                case "FromOADate":
                case "SpecifyKind":
                case "IsLeapYear":
                case "Parse":
                case "ParseExact":
                case "TryParse":
                case "TryParseExact":
                    break;
            }


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