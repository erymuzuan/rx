using System;
using System.ComponentModel.Composition;
using System.Text;
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
            var code = new StringBuilder();
            code.AppendLine(this.GetStatementCode(model, aes.Expression));
            code.AppendLine(".done(function(result)){");

            // in a local declartion
            var local = node.Parent.Parent.Parent.Parent as LocalDeclarationStatementSyntax;
            var methodBlock = node.Parent.Parent.Parent.Parent.Parent as BlockSyntax;
            if (null != methodBlock)
            {
                var index = methodBlock.Statements.IndexOf(local);
                Console.WriteLine("::::{0}::::", index);
                for (int i = index + 1; i < methodBlock.Statements.Count; i++)
                {
                    code.AppendLine("       " + base.GetStatementCode(model, methodBlock.Statements[i]));
                }
            }

            code.AppendLine("});");

            // TODO : wait expression should unwind the stack and create a new state machine
            return code.ToString();
        }

    }
}