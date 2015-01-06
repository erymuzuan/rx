using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class NumberSyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "int", "Int32", "decimal", "Decimal", "float", "Single", "double", "Double" }; }
        }
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
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
            var aguments = node.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                .Single()
                .ChildNodes()
                .OfType<ArgumentSyntax>()
                .Select(x => x.Expression);

            var code = this.Code;
            if (node.Identifier.Text == "Parse")
            {
                var arg = aguments.FirstOrDefault();
                if (null == arg)
                    throw new Exception("Parse must have at least 1 arg");


                code.AppendFormat("parseInt({0})", arg);
            }


            if (node.Identifier.Text == "Max")
                code.Append("Infinity");

            base.VisitIdentifierName(node);
        }


    }
}