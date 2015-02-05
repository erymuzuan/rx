using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ForEachStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ForEachStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.ForEachStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var fesyntax = (ForEachStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendFormat("{0}.each(function({1}) {{", this.EvaluateExpressionCode(fesyntax.Expression) ,fesyntax.Identifier.Text);
            code.AppendLine();


            var block = fesyntax.Statement as BlockSyntax;
            if (null != block)
            {
                var blockCode = this.Compiler.BuildAwaitStatementTree(block.Statements.ToList(), model, true);
                code.AppendLine(blockCode);
            }
            else
            {
                code.AppendLine("       " + this.GetStatementCode(model, fesyntax.Statement));
            }
            code.AppendLine("});");

         



            return code.ToString();
        }
    }
}