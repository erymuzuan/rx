using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class StringMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {
        public override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            return info.Symbol.ContainingType.ToString() == "string";
        }

        protected override string[] ObjectNames
        {
            get { return new[] { "string", "String" }; }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
        }

        protected override SymbolKind[] SymbolKinds
        {
            get { return new[] { SymbolKind.Method }; }
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

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            Console.WriteLine(node);
            base.VisitInvocationExpression(node);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            string code;

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
                case "Length":
                    code = "length";
                    break;
                case "ToUpper":
                    code = "toUpper()";
                    break;
                case "Equals":
                case "CopyTo":
                case "ToCharArray":
                case "Split":
                case "Substring":
                case "TrimStart":
                case "TrimEnd":
                case "IsNormalized":
                case "Normalize":
                case "CompareTo":
                case "Contains":
                case "EndsWith":
                case "IndexOf":
                case "IndexOfAny":
                case "LastIndexOf":
                case "LastIndexOfAny":
                case "PadLeft":
                case "PadRight":
                case "StartsWith":
                case "ToLower":
                case "ToLowerInvariant":
                case "ToUpperInvariant":
                case "ToString":
                case "Clone":
                case "Insert":
                case "Replace":
                case "Remove":
                    code = "/* string." + text + " is not implemented for Javascript compiler */";
                    break;
                case "GetEnumerator":
                case "GetTypeCode":
                case "GetHashCode":
                case "Chars":
                    code = "/* string." + text + " is not supported for Javascript compiler */";
                    break;
                default:
                    code = "/* string." + text + " is not supported for Javascript compiler */";
                    break;
            }

            this.Code.Append(code);
            base.VisitIdentifierName(node);
        }



    }
}