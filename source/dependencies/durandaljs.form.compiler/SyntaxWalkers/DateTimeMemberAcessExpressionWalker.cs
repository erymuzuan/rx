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

        public override bool Filter(SymbolInfo info)
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

            // static methods and propertues
            return symbol.ContainingType.Name == "DateTime" &&
                symbol.ContainingNamespace.Name == "System" &&
                symbol.ContainingAssembly.Name == "mscorlib";
        }
        protected override string[] ObjectNames
        {
            get { return new[] { "DateTime" }; }
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
            else
            {
                // NOTE : assume it's an item property
                var parent = node.Parent;
                if (null != parent && parent.ToString().StartsWith("item."))
                {
                    // Filter again, to see if the node really is DateTime property
                    var sym = this.Filter(this.SemanticModel.GetSymbolInfo(node));
                    if (sym)
                    {
                        this.Code.Clear();
                        this.Code.Append(code);
                        this.Code.Append(text + "().moment()");

                    }
                }
            }

            base.VisitIdentifierName(node);
        }


    }
}