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
            {   //System.Linq.Enumerable
                if (null != prop.ContainingType && prop.ContainingType.Name == "Enumerable")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                           prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "Enumerable" || named.ToString() == "System.Linq.Enumerable?") &&
                       named.ContainingNamespace.Name == "System";

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
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes && node.CSharpKind() == SyntaxKind.SimpleMemberAccessExpression)
            {
                var walker = this.Walkers.Single(x => x.Filter(maes.Expression));
                return walker.Walk(maes.Expression, model);
            }
            return string.Empty;
        }





    }
}