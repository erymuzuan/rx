using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ItemMemberAccessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool Filter(SymbolInfo info)
        {
            var ps = info.Symbol as IParameterSymbol;
            if (null != ps) return ps.Name == "item";

            var prop = info.Symbol as IPropertySymbol;
            if(null != prop)return prop.Name == "item";
            return false;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
                return this.EvaluateExpressionCode(maes.Expression) + "." + this.EvaluateExpressionCode(maes.Name) + "()";

            var invocation = node as InvocationExpressionSyntax;
            if (null != invocation)
            {
                Console.WriteLine("********************");
            }
            var identifier = node as IdentifierNameSyntax;
            if (null != identifier)
            {
                if (identifier.Identifier.Text == "item")
                    return "$data";
            }
            return node.ToFullString() + "//**/";
        }

    }
}