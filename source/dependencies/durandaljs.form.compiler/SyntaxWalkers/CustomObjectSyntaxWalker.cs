﻿using System;
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
    public abstract class CustomObjectSyntaxWalker : CSharpSyntaxWalker
    {

        protected abstract string[] ObjectNames { get; }
        protected abstract SyntaxKind[] Kinds { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected virtual SymbolKind[] SymbolKinds { get { return new SymbolKind[] { }; } }

        protected StringBuilder Code { get; private set; }

        public virtual CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            return null;
        }

        public virtual bool Filter(SymbolInfo info)
        {
            return false;
        }

        public virtual bool Filter(SyntaxNode node, SemanticModel model)
        {
            SemanticModel = model;
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

        protected SemanticModel SemanticModel { get; set; }
        public virtual string Walk(SyntaxNode node, SemanticModel model)
        {
            this.SemanticModel = model;
            var walker = this;

            walker.Code = new StringBuilder();
            walker.SemanticModel = model;
            if(null != model)
                this.Walkers.Where(x => null == x.SemanticModel).ToList().ForEach(x => x.SemanticModel = model);
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
                .Where(x => x.Filter(expression, this.SemanticModel))
                .Select(x => x.Walk(expression, this.SemanticModel))
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

        protected string GetStatementCode(SemanticModel model, SyntaxNode node)
        {
            var code = new StringBuilder();
            var walkers = this.Walkers.Where(x => x.Filter(node, model)).ToList();
            foreach (var w in walkers)
            {
                var f = w.Walk(node, model);
                if (string.IsNullOrWhiteSpace(f)) continue;
                code.AppendLine(f.TrimEnd());
            }
            if (!walkers.Any())
            {
                Console.WriteLine("!!!!!! Cannot find statement walker for " + node.CSharpKind());
            }
            return code.ToString().TrimEnd();
        }


    }
}