using System;
using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ItemMemberAccessExpressionWalker : CustomObjectSyntaxWalker
    {

        protected override string[] ObjectNames
        {
            get { return new[] { "item" }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var text = node.Identifier.Text;
            var code = new StringBuilder();
            if (text == "item")
                code.Append("$data");
            else
                code.AppendFormat(".{0}()", text);

            var model = this.SemanticModel;
            var symbol = model.GetSymbolInfo(node);
            Console.WriteLine(symbol);
            
            this.Code.Append(code);
            base.VisitIdentifierName(node);
        }
    }
}