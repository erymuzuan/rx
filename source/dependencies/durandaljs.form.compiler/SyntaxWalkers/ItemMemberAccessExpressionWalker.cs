using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
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
            var code = new StringBuilder(this.Code.ToString());

            var model = this.Container.SemanticModel;
            var symbol = model.GetSymbolInfo(node);
            var sw = this.Walkers.FirstOrDefault(x => x.Filter(symbol));
            if (null != sw)
            {
                code.Append("." + sw.Walk(node));
            }
            else
            {
                if (text == "item")
                    code.Append("$data");
                else
                    code.AppendFormat(".{0}()", text);
            }

            this.Code.Clear();
            this.Code.Append(code);
            base.VisitIdentifierName(node);
        }
    }
}