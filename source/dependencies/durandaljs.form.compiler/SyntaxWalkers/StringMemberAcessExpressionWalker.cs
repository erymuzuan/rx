using System.ComponentModel.Composition;
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
            const string ARGS = "TODO: get the args";
            string text = node.Identifier.Text;
            if (text == "Trim")
                code = string.Format("String.trim({0})", ARGS);
            if (text == "IsNullOrEmpty")
                code = string.Format("String.isNullOrEmpty({0})", ARGS);
            if (text == "IsNullOrWhiteSpace")
                code = string.Format("String.isNullOrWhiteSpace({0})", ARGS);
            if (text == "Empty")
                code = "''";

            this.Code.Append(code);
            base.VisitIdentifierName(node);
        }


    
    }
}