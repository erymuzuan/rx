using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DecimalSyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "decimal", "Decimal" }; }
        }
        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.SimpleMemberAccessExpression ,
                SyntaxKind.InvocationExpression
            };
            }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
        }

        public override string Walk(SyntaxNode node)
        {
            var walker = this;
            if (!walker.Filter(node)) return string.Empty;


            walker.Visit(node);
            return walker.Code.ToString();
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {

            var code = "";
            var text = node.Identifier.Text;
            switch (text)
            {
                case "Parse":
                    var parseArgs = node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression)
                        .ToList();
                    var parsedNumber = this.EvaluateExpressionCode(parseArgs[0]);
                    code = string.Format("parseFloat({0})", parsedNumber);
                    break;
                case "Round":
                    var roundArgs = node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression)
                        .ToList();
                    var number = roundArgs.FirstOrDefault();
                    var round = "0";
                    if (roundArgs.Count == 2)
                        round = this.EvaluateExpressionCode(roundArgs[1]);
                    if (null == number)
                        throw new Exception("Round must have at least 1 args");
                    code = string.Format("{0}.toFixed({1})", this.EvaluateExpressionCode(number), round);
                    break;
                case "MaxValue":
                    code = "Infinity";
                    break;
                case "MinValue":
                    code = "-Infinity";
                    break;
                case "Zero":
                    code = "0";
                    break;
                case "One":
                    code = "1";
                    break;
                case "MinusOne":
                    code = "-1";
                    break;
                case "ToOACurrency":
                case "FromOACurrency":
                case "Add":
                case "Ceiling":
                case "Compare":
                case "Divide":
                case "Equals":
                case "Floor":
                case "TryParse":
                case "GetBits":
                case "Remainder":
                case "Multiply":
                case "Negate":
                case "Subtract":
                case "ToByte":
                case "ToSByte":
                case "ToInt16":
                case "ToDouble":
                case "ToInt32":
                case "ToInt64":
                case "ToUInt16":
                case "ToUInt32":
                case "ToUInt64":
                case "ToSingle":
                case "Truncate":
                    code = "/* " + text + " is not implemented in the Javascript compiler */";
                    break;
                default:
                    code = "/* " + text + " is not recongnized for Decimal in the Javascript compiler */";
                    break;
            }


            this.Code.Clear();
            this.Code.Append(code);

            base.VisitIdentifierName(node);
        }


    }
}