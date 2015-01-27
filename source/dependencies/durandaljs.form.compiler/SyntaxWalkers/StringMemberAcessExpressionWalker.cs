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

        public override bool Filter(SymbolInfo info)
        {
            if (null == info.Symbol) return false;

            var nts = info.Symbol as INamedTypeSymbol;
            if (null != nts) return nts.ToString() == "System.String";

            return info.Symbol.ContainingType.Name == "string" ||
                info.Symbol.ContainingType.Name == "String";
        }

        protected override string[] ObjectNames
        {
            get { return new[] { "string", "String" }; }
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

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            Console.WriteLine(node);
            base.VisitInvocationExpression(node);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {

            // NOTE : calling this.Evaluate or this.GetArguments will reset this.Code
            var code = this.Code.ToString();
            var text = node.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => string.Equals(x.Metadata.Text, text, StringComparison.InvariantCultureIgnoreCase));
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