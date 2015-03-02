using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class WhileStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.WhileStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.Kind() == SyntaxKind.WhileStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ifsyntax = (WhileStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendLine("while( " + this.EvaluateExpressionCode(ifsyntax.Condition) + "){");


            var block = ifsyntax.Statement as BlockSyntax;
            if (null != block)
            {
                var blockCode = this.Compiler.BuildAwaitStatementTree(block.Statements.ToList(), model, true);
                code.AppendLine(blockCode);
            }
            else
            {
                code.AppendLine("       " + this.GetStatementCode(model, ifsyntax.Statement));
            }
            code.AppendLine("}");




            return code.ToString();
        }
    }
}