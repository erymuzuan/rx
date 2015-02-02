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
        [ImportMany("String", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.String";

            return info.Symbol.ContainingType.Name == "string" ||
                info.Symbol.ContainingType.Name == "String";
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



        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var node2 = node as IdentifierNameSyntax;
            if (null == node2) return string.Empty;

            var text = node2.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => string.Equals(x.Metadata.Text, text, StringComparison.InvariantCultureIgnoreCase));
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node2).ToList();
                var xp = compiler.Value.Compile(node2, argumentList);
                return ("." + xp);
            }

            return string.Empty;


        }



    }
}