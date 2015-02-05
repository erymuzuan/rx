using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class DoStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.DoStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.DoStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ifsyntax = (DoStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendLine("do { ");


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
            code.AppendLine("} while( " + this.EvaluateExpressionCode(ifsyntax.Condition) + ");");




            return code.ToString();
        }
    }
}