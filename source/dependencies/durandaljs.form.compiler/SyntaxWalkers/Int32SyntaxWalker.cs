using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class Int32SyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "int", "Int32" }; }
        }
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
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
                case "MaxValue":
                    code = "Infinity";
                    break;
                case "MinValue":
                    code = "-Infinity";
                    break;
            }


            this.Code.Clear();
            this.Code.Append(code);

            base.VisitIdentifierName(node);
        }


    }
}