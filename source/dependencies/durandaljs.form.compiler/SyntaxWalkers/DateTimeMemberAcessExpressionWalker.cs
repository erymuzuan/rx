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

            // static methods and propertues
            return symbol.ContainingType.Name == "DateTime" &&
                symbol.ContainingNamespace.Name == "System" &&
                symbol.ContainingAssembly.Name == "mscorlib";
        }


        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        public override string Walk(SyntaxNode node2, SemanticModel model)
        {
            var node = (IdentifierNameSyntax)node2;
            var text = node.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node).ToList();
                var xp = compiler.Value.Compile(node, argumentList);


                return ("." + xp);
            }


            return string.Empty;
        }


    }
}