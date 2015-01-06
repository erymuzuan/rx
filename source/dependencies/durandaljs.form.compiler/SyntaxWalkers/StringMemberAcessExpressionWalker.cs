using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class StringMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override string[] ObjectNames
        {
            get { return new[] { "string", "String" }; }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
        }

        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.InvocationExpression,
                SyntaxKind.SimpleMemberAccessExpression
            };
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var code = "";

            // NOTE: assuming there's only 1 argument for these methods
            var args = this.GetArguments(node).Select(this.EvaluateExpressionCode);
            var argumentSyntax = string.Join(", ", args);
            var text = node.Identifier.Text;
            switch (text)
            {
                case "Trim":
                    code = string.Format("String.trim({0})", argumentSyntax);
                    break;
                case "IsNullOrEmpty":
                    code = string.Format("String.isNullOrEmpty({0})", argumentSyntax);
                    break;
                case "IsNullOrWhiteSpace":
                    code = string.Format("String.isNullOrWhiteSpace({0})", argumentSyntax);
                    break;
                case "Empty":
                    code = "''";
                    break;
                default:
                    code = "// Not Supported for " + text;
                    break;
            }

            this.Code.Append(code);
            base.VisitIdentifierName(node);
        }



    }
}