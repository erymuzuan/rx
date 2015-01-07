using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class CustomObjectModel
    {
        public CSharpSyntaxTree SyntaxTree { get; set; }
        public bool IncludeAsParameter { get; set; }
        public string ClassName { get; set; }
        public string IdentifierText { get; set; }
    }
    public abstract class CustomObjectSyntaxWalker : CSharpSyntaxWalker
    {
        [Import]
        public CompilationUnitContainer Container { get; set; }
        protected abstract string[] ObjectNames { get; }
        protected abstract SyntaxKind[] Kinds { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected virtual SymbolKind[] SymbolKinds { get { return new SymbolKind[] { }; } }

        protected StringBuilder Code { get; private set; }

        public virtual CustomObjectModel GetObjectModel(Entity entity)
        {
            return null;
        }

        public virtual bool Filter(SymbolInfo info)
        {
            return false;
        }

        public virtual bool Filter(SyntaxNode node)
        {
            this.Code = new StringBuilder();
            if (!this.Kinds.Contains(node.CSharpKind())) return false;

            if (this.IsPredefinedType)
            {
                var maes5 = node as MemberAccessExpressionSyntax;
                if (null != maes5)
                {
                    var pts = maes5.Expression as PredefinedTypeSyntax;
                    if (null != pts)
                        return this.ObjectNames.Contains(pts.ToString());
                }

                var ies5 = node as InvocationExpressionSyntax;
                if (null != ies5)
                {
                    // TODO : what...???
                }
            }

            if (null == this.ObjectNames)
                return true;

            var maes = node as MemberAccessExpressionSyntax;
            while (null != maes)
            {
                var id = maes.Expression as IdentifierNameSyntax;
                if (null != id && this.ObjectNames.Contains(id.Identifier.Text))
                    return true;

                maes = maes.Expression as MemberAccessExpressionSyntax;
            }



            var ies = node as InvocationExpressionSyntax;
            if (null != ies && this.Kinds.Contains(SyntaxKind.InvocationExpression))
            {
                // check for the name
                if (this.IsPredefinedType)
                {
                    var x = (((MemberAccessExpressionSyntax)ies.Expression).Expression) as
                            PredefinedTypeSyntax;
                    if (null != x && this.ObjectNames.Contains(x.Keyword.ValueText)) return true;
                }
                var maes2 = ies.Expression as MemberAccessExpressionSyntax;
                if (null != maes2)
                {
                    var id2 = maes2.Expression as IdentifierNameSyntax;
                    if (null != id2 && this.ObjectNames.Contains(id2.Identifier.Text)) return true;
                }
            }

            return false;
        }

        public virtual string Walk(SyntaxNode node)
        {
            var walker = this;
            walker.Code = new StringBuilder();
            walker.Visit(node);
            return walker.Code.ToString();
        }

        [ImportMany(RequiredCreationPolicy = CreationPolicy.Shared)]
        public CustomObjectSyntaxWalker[] Walkers { get; set; }

        protected string EvaluateExpressionCode(ExpressionSyntax expression)
        {
            if (null == this.Walkers)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Walkers)
                throw new InvalidOperationException("Failed to load MEF");

            return Walkers
                .Where(x => x.Filter(expression))
                .Select(x => x.Walk(expression))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }


        protected virtual IEnumerable<ExpressionSyntax> GetArguments(SyntaxNode node)
        {
            var parent = node;
            while (null != parent && !parent.ChildNodes().OfType<ArgumentListSyntax>().Any())
            {
                parent = parent.Parent;
            }
            if (null == parent)
                return new ExpressionSyntax[] { };

            return parent.ChildNodes().OfType<ArgumentListSyntax>()
                .Single()
                .ChildNodes()
                .OfType<ArgumentSyntax>()
                .Select(x => x.Expression);
        }
        public class Comparer : IEqualityComparer<CustomObjectSyntaxWalker>
        {
            public bool Equals(CustomObjectSyntaxWalker x, CustomObjectSyntaxWalker y)
            {
                return x.GetType() == y.GetType();
            }

            public int GetHashCode(CustomObjectSyntaxWalker obj)
            {
                return obj.GetType().GetHashCode();
            }
        }

    }
}
