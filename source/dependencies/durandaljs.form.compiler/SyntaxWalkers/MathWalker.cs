using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class MathWalker : CustomObjectSyntaxWalker
    {

        [ImportMany("Math", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.Math";

            var ms = info.Symbol as IMethodSymbol;
            if (null != ms)
                return ms.ContainingType.Name == "Math" && ms.ContainingNamespace.Name == "System";

            return info.Symbol.ContainingType.ToString() == "Math";
        }

        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        public override string Walk(SyntaxNode n, SemanticModel model)
        {
            var node = n as IdentifierNameSyntax;
            if (null == node) return string.Empty;

            var text = node.Identifier.Text;
            if (text == "Math") return "Math";

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node).ToList();
                var xp = compiler.Value.Compile(node, argumentList);

                return xp;
            }
            return string.Empty;
        }


    }
}