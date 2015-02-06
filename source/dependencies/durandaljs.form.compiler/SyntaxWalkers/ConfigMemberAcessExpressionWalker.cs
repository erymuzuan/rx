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
        private const string InterfaceName = "IConfig";

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLinf("   public interface {0}", InterfaceName);
            code.AppendLine("   {");
            code.AppendLine("       bool IsAuthenticated {get;}");
            code.AppendLine("       string[] Roles {get;}");
            code.AppendLine("       string UserName {get;}");
            code.AppendLine("       string ShortDateFormatString {get;}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                InterfaceName = InterfaceName,
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
                return ms.ContainingType.Name == InterfaceName
                    && ms.ContainingAssembly.Name == EVAL;

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