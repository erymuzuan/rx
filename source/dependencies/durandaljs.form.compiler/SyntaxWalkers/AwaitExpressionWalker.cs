using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class AwaitExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.AwaitExpression }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is AwaitExpressionSyntax;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var aes = (AwaitExpressionSyntax)node;
            var code = this.Walkers
                .Where(x => x.Filter(aes.Expression, model))
                .Select(x => x.Walk(aes.Expression, model))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            // TODO : wait expression should unwind the stack and create a new state machine
            code += ";";
            code += "\r\n //TODO : Await should unwind the stack and create a new state machine";
            return code.TrimEnd();
        }
    }
}