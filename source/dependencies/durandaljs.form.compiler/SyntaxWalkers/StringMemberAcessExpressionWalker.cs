﻿using System;
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


        protected override SyntaxKind[] Kinds
        {
            get
            {
                return new[]
            {
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxKind.PredefinedType
            };
            }
        }


        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var code = node.ToFullString();
            if (code.ToLowerInvariant() == "string.empty")
                return "''";

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
            if (text.ToLowerInvariant() == "string") return "String";

            var compiler = this.IdentifierCompilers.LastOrDefault(x => string.Equals(x.Metadata.Text, text, StringComparison.InvariantCultureIgnoreCase));
            if (null != compiler)
            {
                var argumentList = this.GetArguments(id).ToList();
                var xp = compiler.Value.Compile(id, argumentList);
                return xp;
            }

            return "Cannot find compiler for string." + text;


        }



    }
}