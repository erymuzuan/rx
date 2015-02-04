using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ConfigMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        private const string ClassName = "ConfigurationManager";

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLinf("   public class {0}", ClassName);
            code.AppendLine("   {");
            code.AppendLine("       public bool IsAuthenticated {get;}");
            code.AppendLine("       public string[] Roles {get;}");
            code.AppendLine("       public string UserName {get;}");
            code.AppendLine("       public string ShortDateFormatString {get;}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                ClassName = ClassName,
                IdentifierText = "config"
            };
            return com;
        }

        protected override bool Filter(SymbolInfo info)
        {
            var ips = info.Symbol as IParameterSymbol;
            if (null != ips)
                return ips.Name == "config";

            var ms = info.Symbol as IPropertySymbol;
            if (null != ms)
                return ms.ContainingType.Name == ClassName
                    && ms.ContainingAssembly.Name == "eval";

            return false;
        }


        protected override string Walk(IdentifierNameSyntax id, SemanticModel model)
        {
            var text = id.Identifier.Text;
            switch (text)
            {
                case "config": return "config";
                case "IsAuthenticated": return "isAuthenticated";
                case "UserName": return "userName";
                case "Roles": return "roles";
                case "ShortDateFormatString": return "shortDateFormatString";
            }

            return base.Walk(id, model);
        }
    }
}