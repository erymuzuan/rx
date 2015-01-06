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
            get { return new[] {  "decimal", "Decimal" }; }
        }
        protected override SyntaxKind[] Kinds
        {
            get { return new[]
            {
                SyntaxKind.SimpleMemberAccessExpression ,
                SyntaxKind.InvocationExpression
            }; }
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
                    var aguments = node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression);
                    var arg = aguments.FirstOrDefault();
                    if (null == arg)
                        throw new Exception("Parse must have at least 1 arg");
                    code = string.Format("parseInt({0})", this.EvaluateExpressionCode(arg));
                    break;
                case "Round":
                    var args = node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression)
                        .ToList();
                    var number = args.FirstOrDefault();
                    var round = "0";
                    if (args.Count == 2)
                        round = this.EvaluateExpressionCode(args[1]);
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
            }


            this.Code.Clear();
            this.Code.Append(code);

            base.VisitIdentifierName(node);
        }


    }
}