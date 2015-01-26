using System.ComponentModel.Composition;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class SimpleMemberAccessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return null; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            var local = symbol as ILocalSymbol;
            return local != null;
        }


        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var symbol = this.SemanticModel.GetSymbolInfo(node);
            if (this.Filter(symbol))
            {
                var text = node.Identifier.Text;
                var code = new StringBuilder(this.Code.ToString());
                code.Append(text);
                this.Code.Clear();
                this.Code.Append(code);
            }

            base.VisitIdentifierName(node);
        }
    }
}
