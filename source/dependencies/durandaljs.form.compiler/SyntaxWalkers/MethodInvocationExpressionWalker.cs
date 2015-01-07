using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
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

            var code = this.Walkers
                .Where(x => x.Filter(node.Expression))
                .Select(w => w.Walk(node.Expression))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (!string.IsNullOrWhiteSpace(code))
                this.Code.Append(code);

            base.VisitInvocationExpression(node);
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // for array .ContainsMethod
            var arguments = this.GetArguments(node).Select(this.EvaluateExpressionCode).ToList()
                ;

            if (node.Identifier.Text == "Contains")
            {
                this.Code.AppendFormat(".indexOf({0}) > -1", arguments[0]);
            }
            if (node.Identifier.Text == "Count")
            {
                this.Code.Append(".length");
            }

            //var info = this.Container.SemanticModel.GetSymbolInfo(node.Parent);
            //var sw = this.Walkers.FirstOrDefault(x => x.Filter(info));
            //if (null != sw)
            //{
            //    this.Code.Append("." + sw.Walk(node));
            //}

            base.VisitIdentifierName(node);
        }
    }
}