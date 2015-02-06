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
        
        protected override bool Filter(IParameterSymbol parameter)
        {
            return parameter.Name == "logger";
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == "ILogger"
                && method.ContainingAssembly.Name == EVAL;
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
            code.AppendLine("   public interface ILogger");
            code.AppendLine("   {");
            code.AppendLine("       void Info(string message);");
            code.AppendLine("       void Warning(string message);");
            code.AppendLine("       void Error(string message);");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                InterfaceName = "ILogger",
                IdentifierText = "logger"
            };
            return com;
        }



    }
}