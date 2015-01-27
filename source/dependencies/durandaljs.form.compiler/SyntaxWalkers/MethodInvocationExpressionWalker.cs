using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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


            var original = this.Code.ToString();
            var code = PrintDebugInfo(node);
           
            this.Code.Clear();
            this.Code.Append(original);
            this.Code.Append(code);

            base.VisitInvocationExpression(node);
        }

        private string PrintDebugInfo(InvocationExpressionSyntax node)
        {
            var code = new StringBuilder();
            var syntaxWalkers = this.Walkers
                .Where(x => x.Filter(node.Expression, this.SemanticModel));
            foreach (var w in syntaxWalkers)
            {
                var c = w.Walk(node.Expression, this.SemanticModel);
                Console.WriteLine("[ExpressionWalker]{0}\t=> {1}", w.GetType().Name, c);
                code.Append(c);
            }

            var symbolWalkers = from w in this.Walkers
                                let sm = this.SemanticModel.GetSymbolInfo(node.Expression)
                                where w.Filter(sm)
                                select w;
            foreach (var w in symbolWalkers)
            {
                var c = w.Walk(node.Expression, this.SemanticModel);
                Console.WriteLine("[SymbolWalker]{0}\t=> {1}", w.GetType().Name, c);
                code.Append(".");
                code.Append(c);
            }

            return code.ToString();
        }


    }
}