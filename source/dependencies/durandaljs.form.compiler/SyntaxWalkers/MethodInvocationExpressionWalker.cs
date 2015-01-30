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

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ies = (InvocationExpressionSyntax)node;
            
            var code = new StringBuilder();
            var syntaxWalkers = this.Walkers
                .Where(x => x != this)
                .Where(x => x.Filter(ies.Expression, this.SemanticModel))
                .ToArray();
            foreach (var w in syntaxWalkers)
            {
                var c = w.Walk(ies.Expression, this.SemanticModel);
                if (DebuggerHelper.IsVerbose)
                    Console.WriteLine("[ExpressionWalker]{0}\t=> {1}", w.GetType().Name, c);
                code.Append(c);
            }

            var symbolWalkers = from w in this.Walkers
                                where !syntaxWalkers.Contains(w)
                                && w != this
                                let sm = this.GetSymbolInfo(ies.Expression, this.SemanticModel)
                                where sm.Symbol != null
                                where w.Filter(sm)
                                select w;
            foreach (var w in symbolWalkers)
            {
                // if the syntax walker already excute it, then forget it
                var c = w.Walk(ies.Expression, this.SemanticModel);
                if (DebuggerHelper.IsVerbose)
                    Console.WriteLine("[SymbolWalker]{0}\t=> {1}", w.GetType().Name, c);
                code.Append(".");
                code.Append(c);
            }

            return code.ToString();
        }

        private SymbolInfo GetSymbolInfo(ExpressionSyntax node, SemanticModel model)
        {
            try
            {
                return model.GetSymbolInfo(node);
            }
            catch (ArgumentException e)
            {
                if (e.Message != "Syntax node is not within syntax tree") throw;
                return default(SymbolInfo);
            }
        }


    }
}