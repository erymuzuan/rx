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
    public abstract class CustomObjectSyntaxWalker : CSharpSyntaxWalker
    {
        protected abstract SyntaxKind[] Kinds { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected virtual SymbolKind[] SymbolKinds { get { return new SymbolKind[] { }; } }

        protected StringBuilder Code { get; private set; }

        public virtual CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            return null;
        }

        public virtual bool Filter(SyntaxNode node)
        {
            return this.Filter(this.SemanticModel.GetSymbolInfo(node));
        }
        public virtual bool Filter(SymbolInfo info)
        {
            return false;
        }


        public SemanticModel SemanticModel { get; set; }
        public virtual string Walk(SyntaxNode node, SemanticModel model)
        {
            this.SemanticModel = model;
            var walker = this;

            walker.Code = new StringBuilder();
            walker.SemanticModel = model;
            if (null != model)
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

            var symbol = this.SemanticModel.GetSymbolInfo(expression);
            if (null != symbol.Symbol)
            {
                var w = Walkers
                    .SingleOrDefault(x => x.Filter(symbol));
                if (null != w)
                    return w.Walk(expression, this.SemanticModel);

                Console.WriteLine("Cannot find symbol walker for " + expression.CSharpKind());
                Console.WriteLine("Name : " + symbol.Symbol.Name);
                Console.WriteLine("Display : " + symbol.Symbol.ToDisplayString());

            }
            Console.WriteLine("No symbol could be produced for " + expression.CSharpKind());
            Console.WriteLine("expression : " + expression.ToFullString());
            return string.Empty;
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
            var walkers = this.Walkers.Where(x => x.Filter(model.GetSymbolInfo(node))).ToList();
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
