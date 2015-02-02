using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    public class TaskMemberAcessExpressionWalker : CustomObjectSyntaxWalker
    {

        [ImportMany("Task", typeof(IdentifierCompiler), AllowRecomposition = true)]
        public Lazy<IdentifierCompiler, IIdentifierCompilerMetadata>[] IdentifierCompilers { get; set; }

        protected override bool Filter(SymbolInfo info)
        {
            var symbol = info.Symbol;
            if (null == symbol) return false;

            //var t = System.Threading.Tasks.Task.
            var prop = symbol as IPropertySymbol;
            if (prop != null)
            {
                if (null != prop.ContainingType && prop.ContainingType.Name == "Task")
                {
                    return prop.ContainingNamespace.Name == "System" &&
                           prop.ContainingAssembly.Name == "mscorlib";
                }


                var named = prop.Type;
                return (named.Name == "Task" || named.ToString() == "System.Threading.Tasks.Task") &&
                       named.ContainingNamespace.Name == "System";

            }
            var nts = symbol as INamedTypeSymbol;
            if (null != nts)
                return nts.ToString() == "System.Threading.Tasks.Task";

            if (null == symbol.ContainingType) return false;
            if (null == symbol.ContainingNamespace) return false;
            if (null == symbol.ContainingAssembly) return false;
            // static methods and propertues
            return symbol.ContainingType.Name == "Task" &&
                   symbol.ContainingNamespace.ToString() == "System.Threading.Tasks";
        }
     
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.InvocationExpression }; }
        }

        private string m_code;
        public  string Walk2(SyntaxNode node, SemanticModel model)
        {
            var mae = node as MemberAccessExpressionSyntax;
            if (null != mae)
                return "base.Walk(node, model)";
            var awaitExpression = node as AwaitExpressionSyntax;
            if (null != awaitExpression)
                return "yyyyyyy";

            var invocationExpress = node as InvocationExpressionSyntax;
            if (null != invocationExpress && node.ToFullString().Contains("Task."))
            {
                return m_code;
            }
            return "xxxx";
        }

        public override string Walk(SyntaxNode node2, SemanticModel model)
        {
            var node = (IdentifierNameSyntax)node2;
            var text = node.Identifier.Text;

            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(node).ToList();
                m_code = compiler.Value.Compile(node, argumentList);
            }
            return string.Empty;
        }
     


    }
}