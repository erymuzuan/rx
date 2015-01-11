using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class AddSyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return null; }
        }
        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
                {
                    SyntaxKind.AddExpression 
                };
            }
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var bes = node as BinaryExpressionSyntax;
            if (null == bes) return null;
            return this.EvaluateExpressionCode(bes.Left) + " + " + this.EvaluateExpressionCode(bes.Right);
        }
    }
}