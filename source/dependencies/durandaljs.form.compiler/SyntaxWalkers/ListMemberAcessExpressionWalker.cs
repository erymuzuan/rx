using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class ListMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        [ImportMany("List", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;

            var numbers = new[] { 1, 3, 4, 6, 7, 8, 6 };
            var list = numbers.ToList();
            Console.WriteLine(list.GetType().FullName);
            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {   //System.Linq.Enumerable
                if (null != prop.ContainingType && prop.ContainingType.Name == "List")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                           prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "List" || named.ToString() == "System.Collections.Generic") &&
                       named.ContainingNamespace.Name == "System";

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.Collections.Generic.List`";

            // static methods and propertues
            return symbol.ContainingType.Name == "List" &&
                   symbol.ContainingNamespace.ToString() == "System.Collections.Generic";
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

            try
            {
                var symbol = this.SemanticModel.GetSymbolInfo(node);
                if (!this.Filter(symbol))
                {
                    base.VisitIdentifierName(node);
                    return;

                }
            }
            catch (ArgumentException e)
            {
                if (e.Message != "Syntax node is not within syntax tree") throw;
            }

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