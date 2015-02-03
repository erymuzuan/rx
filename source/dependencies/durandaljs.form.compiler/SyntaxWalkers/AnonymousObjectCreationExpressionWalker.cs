using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class AnonymousObjectCreationExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.AnonymousObjectCreationExpression }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.AnonymousObjectCreationExpression;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var anonymous = (AnonymousObjectCreationExpressionSyntax)node;
            var code = new StringBuilder();

            var initializers = from t in anonymous.Initializers
                               let expression = t.Expression
                               let w = this.GetCode(expression, model)
                               select string.Format("\t{0} : {1}", t.NameEquals.Name.Identifier.Text, w);
            code.AppendLine(string.Join(",\r\n", initializers));


            return "{\r\n" + code + "}";
        }

        private string GetCode(SyntaxNode node, SemanticModel model)
        {
            var code = new StringBuilder();
            var syntaxWalkers = this.Walkers
                .Where(x => x.Filter(node))
                .ToArray();
            foreach (var w in syntaxWalkers)
            {
                return w.Walk(node, model);
            }

            var symbolWalkers = from w in this.Walkers
                                where !syntaxWalkers.Contains(w)
                                && w.Filter(node)
                                select w;
            foreach (var w in symbolWalkers)
            {
                return w.Walk(node, model);
            }

            return code.ToString();
        }
    }
}