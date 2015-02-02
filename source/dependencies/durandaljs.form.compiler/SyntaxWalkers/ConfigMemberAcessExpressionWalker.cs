﻿using System;
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
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace " + project.DefaultNamespace);
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

        protected override bool Filter(SymbolInfo info)
        {
            var ips = info.Symbol as IParameterSymbol;
            if (null != ips)
                return ips.Name == "config";

            var ms = info.Symbol as IPropertySymbol;
            if (null != ms)
                return ms.ContainingType.Name == "ConfigurationManager"
                    && ms.ContainingAssembly.Name == "eval";

            return false;
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                var exp = this.EvaluateExpressionCode(maes.Expression);
                var name = this.EvaluateExpressionCode(maes.Name);
                if (string.IsNullOrWhiteSpace(exp))
                    return name;
                return exp + "." + name;
            }


            var id = node as IdentifierNameSyntax;
            if (null == id)
            {
                var w = this.GetWalker(node, true);
                if (null != w) return w.Walk(node, model);
                throw new Exception("Int32 " + node.CSharpKind());
            }
            var text = id.Identifier.Text;
            if (text == "config") return "config";


            if (text == "IsAuthenticated") return "isAuthenticated";
            if (text == "UserName") return "userName";
            if (text == "Roles") return "roles";

            return "config." + text + " is not yet";

        }


    }
}