using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LoadOperationMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }
        protected override bool Filter(IPropertySymbol prop)
        {
            return prop.ContainingType.Name == "LoadOperation"
                && prop.ContainingAssembly.Name == "domain.sph";
        }

        protected override string Walk(IdentifierNameSyntax id, SemanticModel model)
        {
            var text = id.Identifier.Text;
            switch (text)
            {
                case "ItemCollection": return "itemCollection";
                case "CurrentPage": return "currentPage";
                case "PageSize": return "pageSize";
                case "Filter": return "filter";
                case "TotalRows": return "totalRows";
                case "NextSkipToken": return "nextSkipToken";
            }

            return base.Walk(id, model);
        }
    }
}