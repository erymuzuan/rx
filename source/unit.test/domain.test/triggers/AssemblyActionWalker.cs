using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace domain.test.triggers
{
    public class AssemblyActionWalker : CSharpSyntaxWalker
    {
        public string MethodName { get; set; }
        public AssemblyActionWalker(string methodName)
        {
            this.MethodName = methodName;
            Arguments = new List<ExpressionSyntax>();
        }

        public List<ExpressionSyntax> Arguments { get; private set; }
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {

            var member = node.Expression as MemberAccessExpressionSyntax;
            if (member != null)
            {
                var type = member.Expression as IdentifierNameSyntax;
                if (type != null && type.Identifier.Text == "k" && member.Name.Identifier.Text == this.MethodName)
                {
                    foreach (var arg in node.ArgumentList.Arguments)
                    {
                        Arguments.Add(arg.Expression);

                    }
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}