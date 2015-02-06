using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class AppExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string APPLICATION_HELPER = "IApplicationHelper";
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLinf("   public interface {0}", APPLICATION_HELPER);
            code.AppendLine("   {");
            code.AppendLine("       Task<string> ShowMessageAsync(string message, string[] buttons);");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                InterfaceName = APPLICATION_HELPER,
                IdentifierText = "app"
            };
            return com;
        }


        protected override bool Filter(IParameterSymbol parameter)
        {
            return parameter.Name == "app";
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == APPLICATION_HELPER
                && method.ContainingAssembly.Name == EVAL;
        }

        protected override string InferredTypeName
        {
            get { return APPLICATION_HELPER; }
        }
    }
}