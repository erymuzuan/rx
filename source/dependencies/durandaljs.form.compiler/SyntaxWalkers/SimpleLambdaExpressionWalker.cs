using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class SimpleLambdaExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new string[] { }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleLambdaExpression }; }
        }

        public override bool Filter(SyntaxNode node, SemanticModel model)
        {
            return node is SimpleLambdaExpressionSyntax;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var code = new StringBuilder();
            var les = (SimpleLambdaExpressionSyntax)node;

            var walker = this.Walkers.SingleOrDefault(x => x.Filter(les.Body, model));
            code.AppendFormat(" function({0}){{ ", les.Parameter.Identifier.Text);
            if (null != walker) code.AppendFormat("return {0};",walker.Walk(les.Body, model));
            code.Append(" }");

            return code.ToString();
        }
    }
}