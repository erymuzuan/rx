using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public partial class BooleanExpressionCompiler
    {
        private CSharpSyntaxTree ConfigObjectModel(Entity entity)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace Bespoke." + ConfigurationManager.ApplicationName + "_" + entity.Id + ".Domain");
            code.AppendLine("{");
            code.AppendLine("   public class ConfigurationManager");
            code.AppendLine("   {");
            code.AppendLine("       public bool IsAuthenticated {get;}");
            code.AppendLine("       public string[] Roles {get;}");
            code.AppendLine("       public string UserName {get;}");
            code.AppendLine("   }");
            code.AppendLine("}");
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString());
        }

        class ConfigMemberAcessExpressionWalker : CSharpSyntaxWalker
        {
            private readonly StringBuilder m_code = new StringBuilder();
            internal static string Walk(SyntaxNode node)
            {

                if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression) return string.Empty;
                // NOTE : assuming there's only one member access config.Roles
                var maes = ((MemberAccessExpressionSyntax)node).Expression as MemberAccessExpressionSyntax;
                if (null != maes) return "config.roles";
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

                    if (node.Identifier.Text == "config")
                        m_code.Append(".");
                }
                base.VisitIdentifierName(node);
            }


        }

    }
}
