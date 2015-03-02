using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ForStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.ForStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.ForStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var fesyntax = (ForStatementSyntax)node;
            var code = new StringBuilder();
            var loop = fesyntax.Declaration.Variables[0];
            code.AppendFormat("for ({0} = {1}; {2}; {3}) {{",
                loop.Identifier.Text,
                this.EvaluateExpressionCode(loop.Initializer.Value),
                this.EvaluateExpressionCode(fesyntax.Condition),
                this.EvaluateExpressionCode(fesyntax.Incrementors[0])
                );
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
            code.AppendLine("}");





            return code.ToString();
        }
    }
}