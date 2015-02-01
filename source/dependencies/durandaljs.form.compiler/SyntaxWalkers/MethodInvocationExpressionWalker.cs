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


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ies = (InvocationExpressionSyntax)node;

            var code = new StringBuilder();
            var walkers = this.Walkers.Where(x => x.Filter(ies.Expression)).ToList();
            if (walkers.Count > 1)
                foreach (var v in walkers)
                {
                    Console.WriteLine(node.CSharpKind() + " ----" + v.GetType().Name);
                    Console.WriteLine(node.ToFullString());
                }
            var w = this.Walkers.LastOrDefault(x => x.Filter(ies.Expression));
            if (null == w)
                throw new InvalidOperationException(string.Format("Cannot find walker for {0} => {1}", ies.CSharpKind(), ies.ToFullString()));


            var c = w.Walk(ies.Expression, model);
            if (DebuggerHelper.IsVerbose)
                Console.WriteLine("[ExpressionWalker]{0}\t=> {1}", w.GetType().Name, c);
            code.Append(c);

            return code.ToString();
        }




    }
}