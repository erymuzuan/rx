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
        private readonly StringBuilder m_code = new StringBuilder();

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.InvocationExpression }; }
        }

        protected override string[] ObjectNames
        {
            get { return null; }
        }

        public override string Walk(SyntaxNode node)
        {
            var ies = node as InvocationExpressionSyntax;
            if (null == ies) return string.Empty;

            var walker = new MethodInvocationExpressionWalker();
            walker.Visit(node);
            return walker.m_code.ToString();
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
                m_code.Append(code);

            base.VisitInvocationExpression(node);
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // for array .ContainsMethod
            var arguments = this.GetArguments(node).Select(this.EvaluateExpressionCode).ToList()
                ;
            if (node.Identifier.Text == "Contains")
            {
                m_code.AppendFormat(".indexOf({0}) > -1", arguments[0]);
            }
            if (node.Identifier.Text == "Count")
            {
                m_code.Append(".length");
            }
            base.VisitIdentifierName(node);
        }
    }
}