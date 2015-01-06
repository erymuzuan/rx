using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{

    public abstract class CustomObjectSyntaxWalker : CSharpSyntaxWalker
    {
        protected abstract string[] ObjectNames { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected StringBuilder Code { get; private set; }
        protected bool Filter(SyntaxNode node)
        {
            this.Code = new StringBuilder();
            if (node.CSharpKind() != SyntaxKind.SimpleMemberAccessExpression) return false;

            if (this.IsPredefinedType)
            {
                var pts = ((MemberAccessExpressionSyntax)node).Expression as PredefinedTypeSyntax;
                if (null != pts)
                    return this.ObjectNames.Contains(pts.ToString());
            }

            var identifier = ((MemberAccessExpressionSyntax)node).Expression as IdentifierNameSyntax;
            if (null == identifier) return false;
            if (!this.ObjectNames.Contains(identifier.Identifier.Text)) return false;

            return true;
        }

        protected string EvaluateExpressionCode(ExpressionSyntax expression)
        {
            var value = ItemMemberAccessExpressionWalker.Walk(expression);
            if (!string.IsNullOrWhiteSpace(value))
                return (value);
            value = ConfigMemberAcessExpressionWalker.Walk(expression);
            if (!string.IsNullOrWhiteSpace(value))
                return (value);
            value = DateTimeMemberAcessExpressionWalker.Walk(expression);
            if (!string.IsNullOrWhiteSpace(value))
                return (value);
            
        }


    }
}
