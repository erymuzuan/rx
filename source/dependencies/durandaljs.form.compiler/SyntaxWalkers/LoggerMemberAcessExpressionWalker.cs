﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LoggerMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        [ImportMany("Logger", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool Filter(SymbolInfo info)
        {
            var ips = info.Symbol as IParameterSymbol;
            if (null != ips)
                return ips.Name == "logger";

            var ms = info.Symbol as IMethodSymbol;
            if (null != ms)
                return ms.ContainingType.Name == "Logger"
                    && ms.ContainingAssembly.Name == "eval";

            return false;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                return this.EvaluateExpressionCode(maes.Expression) + "." + this.EvaluateExpressionCode(maes.Name);
            }
            var ins = (IdentifierNameSyntax)node;
            var text = ins.Identifier.Text;

            if (text == "logger") return "logger";

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(ins).ToList();
                var xp = compiler.Value.Compile(ins, argumentList);
                return xp;

            }

            return string.Empty;
        }




        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLine("   public class Logger");
            code.AppendLine("   {");
            code.AppendLine("       public void Info(string message){}");
            code.AppendLine("       public void Warning(string message){}");
            code.AppendLine("       public void Error(string message){}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                ClassName = "Logger",
                IdentifierText = "logger"
            };
            return com;
        }



    }
}