using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class TaskMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string TASK = "Task";
        public const string SYSTEM_THREADING_TASKS = "System.Threading.Tasks";

        protected override bool Filter(IPropertySymbol prop)
        {
            if (null != prop.ContainingType && prop.ContainingType.Name == TASK)
            {
                return prop.ContainingNamespace.Name == SYSTEM &&
                       prop.ContainingAssembly.Name == MSCORLIB;
            }

            return false;
        }

        protected override string InferredTypeName
        {
            get { return TASK; }
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == TASK
                && method.ContainingNamespace.ToString() == SYSTEM_THREADING_TASKS;
        }

        protected override bool Filter(INamedTypeSymbol named)
        {
            return named.ToString() == SYSTEM_THREADING_TASKS + "." + TASK;
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

    }
}