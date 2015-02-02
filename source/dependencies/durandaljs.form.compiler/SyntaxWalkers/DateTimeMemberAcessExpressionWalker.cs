using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DateTimeMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        [ImportMany("DateTime", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;


            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {
                if (null != prop.ContainingType && prop.ContainingType.Name == "DateTime")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                        prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "DateTime" || named.ToString() == "System.DateTime?") &&
                    named.ContainingNamespace.Name == "System" &&
                    named.ContainingAssembly.Name == "mscorlib";

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.DateTime";

            // static methods and properties
            return symbol.ContainingType.Name == "DateTime" &&
                symbol.ContainingNamespace.Name == "System" &&
                symbol.ContainingAssembly.Name == "mscorlib";
        }


        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
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

            var id = (IdentifierNameSyntax)node;
            var text = id.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(id).ToList();
                var xp = compiler.Value.Compile(id, argumentList);


                return ("." + xp);
            }

            var w = this.GetWalker(model.GetSymbolInfo(node), true);
            if (null != w) return w.Walk(node, model);

            throw new Exception("Cannot find  a compiler for DateTime." + text);
        }


    }
}