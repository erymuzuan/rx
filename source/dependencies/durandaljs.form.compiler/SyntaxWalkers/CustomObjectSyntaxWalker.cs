using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{

    public abstract class CustomObjectSyntaxWalker : CSharpSyntaxWalker
    {
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
        protected abstract string[] ObjectNames { get; }
        protected abstract SyntaxKind[] Kinds { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected StringBuilder Code { get; private set; }

        public virtual CSharpSyntaxTree GetObjectModel(Entity entity)
        {
            return null;
        }

        public bool Filter(SyntaxNode node)
        {
            this.Code = new StringBuilder();
            if (!this.Kinds.Contains(node.CSharpKind())) return false;

            if (this.IsPredefinedType)
            {
                var pts = ((MemberAccessExpressionSyntax)node).Expression as PredefinedTypeSyntax;
                if (null != pts)
                    return this.ObjectNames.Contains(pts.ToString());
            }

            if (null == this.ObjectNames)
                return true;

            var maes = node as MemberAccessExpressionSyntax;
            if (null != maes)
            {
                var identifier = maes.Expression as IdentifierNameSyntax;
                if (null == identifier) return false;
                if (!this.ObjectNames.Contains(identifier.Identifier.Text)) return false;
            }

            var ies = node as InvocationExpressionSyntax;
            if (null != ies && this.Kinds.Contains(SyntaxKind.InvocationExpression))
            {
                // check for the name
                return true;
            }

            return true;
        }

        public virtual string Walk(SyntaxNode node)
        {
            var walker = this.Clone();
            walker.Code = new StringBuilder();
            walker.Visit(node);
            return walker.Code.ToString();
        }

        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
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


    }
}
