using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LoggerMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

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

        protected override string InferredTypeName
        {
            get { return "Logger"; }
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