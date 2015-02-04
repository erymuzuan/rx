using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class ItemMemberAccessExpressionWalker : CustomObjectSyntaxWalker
    {
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression }; }
        }

        protected override bool Filter(SymbolInfo info)
        {
            var ps = info.Symbol as IParameterSymbol;
            if (null != ps) return ps.Name == "item";

            var prop = info.Symbol as IPropertySymbol;
            if (null != prop)
            {
                if (prop.Name == "item") return true;
                var domain = prop.ContainingType.BaseType.Name == typeof(Entity).Name
                             || prop.ContainingType.BaseType.Name == typeof (DomainObject).Name;
                var dll = prop.ContainingAssembly.Name == "eval" || prop.ContainingAssembly.Name == typeof(DomainObject).Assembly.GetName().Name;

                return dll && domain;
            }
            return false;
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
                return this.EvaluateExpressionCode(maes.Expression) + "." + this.EvaluateExpressionCode(maes.Name);
      

            var id = node as IdentifierNameSyntax;
            if (null != id)
            {
                if (id.Identifier.Text == "item")
                    return "$data";
                return id.Identifier.Text + "()" + this.GetSymbolExtension(node, model);
            }
            return "/* item should not get  " + node.CSharpKind() + ":"+ node.ToFullString() + "*/";
        }

        private string GetSymbolExtension(SyntaxNode node, SemanticModel model)
        {
            var info = model.GetSymbolInfo(node);
            var prop = info.Symbol as IPropertySymbol;
            if (prop != null)
            {
                var named = prop.Type;
                return (named.Name == "DateTime" || named.ToString() == "System.DateTime?") &&
                    named.ContainingNamespace.Name == "System" &&
                    named.ContainingAssembly.Name == "mscorlib" ? ".moment()" : "";

            }

            return string.Empty;

        }
    }
}