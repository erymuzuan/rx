using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class LinqEnumerableMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        [ImportMany("Enumerable", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;


            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {
                if (null != prop.ContainingType && prop.ContainingType.Name == "Enumerable")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                           prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "Enumerable" || named.ToString() == "System.Linq.Enumerable?") &&
                       named.ContainingNamespace.Name == "System";

            }

            var ms = symbol as IMethodSymbol;
            if (ms != null)
            {
                return ms.ContainingType.Name == "Enumerable"
            && ms.ContainingNamespace.ToString() == "System.Linq";

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.Linq.Enumerable";

            // static methods and propertues
            return symbol.ContainingType.Name == "Enumerable" &&
                   symbol.ContainingNamespace.ToString() == "System.Linq";
        }


        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.IdentifierName, SyntaxKind.InvocationExpression }; }
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
                return w.Walk(node, model);
            }
            var text = id.Identifier.Text;
            var compiler = this.IdentifierCompilers.LastOrDefault(x => string.Equals(x.Metadata.Text, text, StringComparison.InvariantCultureIgnoreCase));
            if (null != compiler)
            {
                var argumentList = this.GetArguments(id).ToList();
                var xp = compiler.Value.Compile(id, argumentList);
                return xp;
            }


            return "Enumerable." + text;
        }

    }
}