using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class Int32SyntaxWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool IsPredefinedType
        {
            get { return true; }
        }

        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.Int32";

            if (null == info.Symbol.ContainingType) return false;
            if (null == info.Symbol.ContainingNamespace) return false;
            if (null == info.Symbol.ContainingAssembly) return false;

            return info.Symbol.ContainingType.Name == "int" ||
                info.Symbol.ContainingType.Name == "Int32";
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        { 
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                var exp = this.EvaluateExpressionCode(maes.Expression);
                var name = this.EvaluateExpressionCode(maes.Name);
                if (string.IsNullOrWhiteSpace(exp))
                    return name;
                return exp + "." + name;
            }


            var id = node as IdentifierNameSyntax;
            if (null == id)
            {
                var w = this.GetWalker(node, true);
                if (null != w) return w.Walk(node, model);
                throw new Exception("Cannot find walker for Int32 " + node.CSharpKind() + " : " + node.ToFullString());
            }


            var text = id.Identifier.Text;
            switch (text)
            {
                case "Parse":
                    var aguments = id.Parent.Parent.ChildNodes().OfType<ArgumentListSyntax>()
                        .Single()
                        .ChildNodes()
                        .OfType<ArgumentSyntax>()
                        .Select(x => x.Expression);
                    var arg = aguments.FirstOrDefault();
                    if (null == arg)
                        throw new Exception("Parse must have at least 1 arg");
                    return string.Format("parseInt({0})", this.EvaluateExpressionCode(arg));
                case "MaxValue":
                    return "Infinity";
                case "MinValue":
                    return "-Infinity";
            }


            return string.Empty;
        }


    }
}