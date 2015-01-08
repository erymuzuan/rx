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
        protected override string[] ObjectNames
        {
            get { return new[] { "config" }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(Entity entity)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace Bespoke." + ConfigurationManager.ApplicationName + "_" + entity.Id + ".Domain");
            code.AppendLine("{");
            code.AppendLine("   public class ConfigurationManager");
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
                ClassName = "ConfigurationManager",
                IdentifierText = "config"
            };
            return com;
        }

        private readonly StringBuilder m_code = new StringBuilder();
        public override string Walk(SyntaxNode node)
        {
            var maes = ((MemberAccessExpressionSyntax)node).Expression as MemberAccessExpressionSyntax;
            if (null != maes
                && node.ToString().StartsWith("config.Roles")
                && maes.Name.Identifier.Text == "Roles") return "config.roles";


            var identifier = ((MemberAccessExpressionSyntax)node).Expression as IdentifierNameSyntax;
            if (null == identifier) return string.Empty;
            if (identifier.Identifier.Text != "config")
                return string.Empty;

            var walker = new ConfigMemberAcessExpressionWalker();
            walker.Visit(node);
            return walker.m_code.ToString();
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.Parent.GetText().ToString().StartsWith("config."))
            {
                if (node.Identifier.Text == "config")
                    m_code.Append("config");
                if (node.Identifier.Text == "IsAuthenticated")
                    m_code.Append("isAuthenticated");
                if (node.Identifier.Text == "UserName")
                    m_code.Append("userName");
                if (node.Identifier.Text == "Roles")
                    m_code.Append("roles");
                if (node.Identifier.Text == "ShortDateFormatString")
                    m_code.Append("shortDateFormatString");
                if (node.Identifier.Text == "Length")
                    m_code.Append("xxxxxxxx");

                if (node.Identifier.Text == "config")
                    m_code.Append(".");
            }
            base.VisitIdentifierName(node);
        }


    }
}