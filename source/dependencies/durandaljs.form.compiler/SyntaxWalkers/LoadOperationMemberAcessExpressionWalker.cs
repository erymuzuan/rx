using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LoadOperationMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string CLASS_NAME = "ILoadOperation";
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }
        protected override bool Filter(IPropertySymbol prop)
        {
            return prop.ContainingType.Name == CLASS_NAME
                && prop.ContainingAssembly.Name == EVAL;
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("using Bespoke.Sph.Domain; " );
            code.AppendLinf("using {0}; ", ListMemberAcessExpressionWalker.SYSTEM_COLLECTION_GENERIC );
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLinf("   public interface {0}<T> where T : Entity ", CLASS_NAME);
            code.AppendLine("   {");
            code.AppendLine("       List<T> ItemCollection {get;}");
            code.AppendLine("       string NextSkipToken {get;}");
            code.AppendLine("       string Filter {get;}");
            code.AppendLine("       int CurrentPage{get;}");
            code.AppendLine("       int PageSize{get;}");
            code.AppendLine("       int TotalRows{get;}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = false,
                InterfaceName = CLASS_NAME
            };
            return com;
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