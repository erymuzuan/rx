using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class MethodInvocationExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.InvocationExpression }; }
        }

        protected override string[] ObjectNames
        {
            get { return null; }
        }


        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (null == this.Walkers)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Walkers)
                throw new InvalidOperationException("MEF!!!");

            var sb = this.Code.ToString();
            var ok = "";

            var code = this.Walkers
                .Where(x => x.Filter(node.Expression, this.SemanticModel))
                .Select(w => w.Walk(node.Expression, this.SemanticModel))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (!string.IsNullOrWhiteSpace(code))
                ok = code;

            //code = this.Walkers
            //   .Where(x => x.Filter(this.SemanticModel.GetSymbolInfo(node.Expression)))
            //   .Select(w => w.Walk(node.Expression, this.SemanticModel))
            //   .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            //if (!string.IsNullOrWhiteSpace(code))
            //    ok = code;

            this.Code.Clear();
            this.Code.Append(sb);
            this.Code.Append(ok);


            base.VisitInvocationExpression(node);
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // for array .ContainsMethod
            var code = this.Code.ToString();
            var arguments = this.GetArguments(node)
                .Select(this.EvaluateExpressionCode)
                .ToList();

            if (node.Identifier.Text == "Contains")
            {
                this.Code.AppendFormat(".indexOf({0}) > -1", arguments[0]);
            }
            if (node.Identifier.Text == "Count")
            {
                this.Code.Append(".length");
            }

            var info = this.SemanticModel.GetSymbolInfo(node);
            var sw = this.Walkers.FirstOrDefault(x => x.Filter(info));
            if (null != sw)
            {
                this.Code.Append("." + sw.Walk(node, this.SemanticModel));
            }

            this.Code.Clear();
            this.Code.Append(code);

            base.VisitIdentifierName(node);
        }
    }
}