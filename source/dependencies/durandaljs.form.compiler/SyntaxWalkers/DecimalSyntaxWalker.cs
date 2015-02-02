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


        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == typeof(decimal).FullName;

            if (null == info.Symbol.ContainingType) return false;
            if (null == info.Symbol.ContainingNamespace) return false;
            if (null == info.Symbol.ContainingAssembly) return false;

            return info.Symbol.ContainingType.Name == "decimal" ||
                info.Symbol.ContainingType.Name == "Decimal";
        }



        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                var exp = this.EvaluateExpressionCode(maes.Expression);
                var name = this.EvaluateExpressionCode(maes.Name);
                if (string.IsNullOrWhiteSpace(exp))
                    return name;
                return exp + "." + name;
            }

            var id = node as IdentifierNameSyntax;
            if (null == id)
            {
                var w = this.GetWalker(node, true);
                return w.Walk(node, model);
            }

            string code;
            var text = id.Identifier.Text;
            switch (text)
            {
                case "Parse":
                    var parseArgs = id.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression)
                        .ToList();
                    var parsedNumber = this.EvaluateExpressionCode(parseArgs[0]);
                    code = string.Format("parseFloat({0})", parsedNumber);
                    break;
                case "Round":
                    var roundArgs = id.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
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


            return code;
        }


    }
}