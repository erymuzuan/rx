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

        public override bool Filter(SymbolInfo info)
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
        protected override string[] ObjectNames
        {
            get { return new[] { "Enumerable" }; }
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // NOTE : calling this.Evaluate or this.GetArguments will reset this.Code
            var code = this.Code.ToString();
            var text = node.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node).ToList();
                var xp = compiler.Value.Compile(node, argumentList);
                this.Code.Clear();
                this.Code.Append(code);
                if (string.IsNullOrWhiteSpace(code))
                    this.Code.Append(xp);
                else
                    this.Code.Append("." + xp);
            }

            base.VisitIdentifierName(node);
        }


    }
}