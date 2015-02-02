using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class LiteralExpressionWalker : CustomObjectSyntaxWalker
    {
        private const string Value = "";

        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.TrueLiteralExpression,
                SyntaxKind.StringLiteralExpression, 
                SyntaxKind.FalseLiteralExpression, 
                SyntaxKind.NumericLiteralExpression, 
                SyntaxKind.CharacterLiteralExpression, 
                SyntaxKind.NullLiteralExpression
            };
            }
        }

        public override bool Filter(SyntaxNode node)
        {
            return Kinds.Contains(node.CSharpKind());
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var literal = (LiteralExpressionSyntax)node;
            var kind = node.CSharpKind();
            switch (kind)
            {
                case SyntaxKind.TrueKeyword: return "true";
                case SyntaxKind.TrueLiteralExpression: return "true";
                case SyntaxKind.FalseKeyword: return "false";
                case SyntaxKind.FalseLiteralExpression: return "false";
                case SyntaxKind.NullLiteralExpression: return "null";
                case SyntaxKind.NumericLiteralExpression: return string.Format("{0}", literal.Token.Value);
                case SyntaxKind.StringLiteralExpression: return string.Format("'{0}'", literal.Token.Value);
                case SyntaxKind.StringLiteralToken: return string.Format("'{0}'", literal.Token.Value);
                case SyntaxKind.NumericLiteralToken: return string.Format("{0}", literal.Token.Value);
            }
            return string.Empty;
        }


    }
}