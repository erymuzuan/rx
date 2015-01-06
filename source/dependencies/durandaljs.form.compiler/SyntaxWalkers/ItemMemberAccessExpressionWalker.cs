using System.ComponentModel.Composition;
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
            if (node.Identifier.Text == "item")
                this.Code.Append("$data");
            if (node.Parent.GetText().ToString().StartsWith("item.") && node.Identifier.Text != "item")
                this.Code.Append(node.Identifier.Text + "()");
            if (node.Identifier.Text == "item")
                this.Code.Append(".");
            base.VisitIdentifierName(node);
        }
    }
}