using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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
            get { return null;}
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
            var args = "TODO://" + this;
            if (node.Identifier.Text == "Contains")
            {
                m_code.AppendFormat(".indexOf({0}) > -1", string.Join(", ", args));
            }
            base.VisitIdentifierName(node);
        }
    }
}