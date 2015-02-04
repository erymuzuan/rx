using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs.SyntaxWalkers
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class IfStatementWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.IfStatement }; }
        }

        public override bool Filter(SyntaxNode node)
        {
            return node.CSharpKind() == SyntaxKind.IfStatement;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var ifsyntax = (IfStatementSyntax)node;
            var code = new StringBuilder();
            code.AppendLine("if( " + this.EvaluateExpressionCode(ifsyntax.Condition) + "){");


            var block = ifsyntax.Statement as BlockSyntax;
            if (null != block)
            {
                foreach (var st in block.Statements)
                {
                    code.AppendLine("       " + this.GetStatementCode(model, st) );
                }
            }
            else
            {
                code.AppendLine("       " + this.GetStatementCode(model, ifsyntax.Statement));
            }
            code.AppendLine("}");

            if (ifsyntax.Else == null) return code.ToString();


            code.AppendLine("else {");
            var elseBlock = ifsyntax.Else.Statement as BlockSyntax;
            if (null != elseBlock)
            {
                foreach (var st in elseBlock.Statements)
                {
                    code.AppendLine("       " + this.GetStatementCode(model, st) );
                }
            }
            else
            {
                code.AppendLine("       " + this.GetStatementCode(model, ifsyntax.Else.Statement));
            }
            code.AppendLine("}");


            return code.ToString();
        }
    }
}