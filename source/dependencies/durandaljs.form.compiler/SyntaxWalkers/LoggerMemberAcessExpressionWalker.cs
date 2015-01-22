using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LoggerMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "logger" }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.InvocationExpression }; }
        }

        private readonly StringBuilder m_code = new StringBuilder("logger.");

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.Identifier.Text == "Info")
                m_code.Append("info");
            if (node.Identifier.Text == "Warning")
                m_code.Append("warning");
            if (node.Identifier.Text == "Error")
                m_code.Append("error");


            var args = "TODO" + this;
            m_code.AppendFormat("({0})", args);

            base.VisitIdentifierName(node);
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var walker = this;
            walker.Visit(node);
            return walker.m_code.ToString();
        }
    }
}