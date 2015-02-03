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
    public abstract class CustomObjectSyntaxWalker
    {
        private SemanticModel m_semanticModel;
        protected abstract SyntaxKind[] Kinds { get; }
        protected virtual bool IsPredefinedType { get { return false; } }
        protected virtual SymbolKind[] SymbolKinds { get { return new SymbolKind[] { }; } }


        public virtual CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            return null;
        }

        public virtual bool Filter(SyntaxNode node)
        {
            try
            {
                if (!this.Kinds.Contains(node.CSharpKind()))
                    return false;
                var info = this.SemanticModel.GetSymbolInfo(node);
                return this.Filter(info);
            }
            catch (ArgumentException e)
            {
                var model = this.Compilation.GetSemanticModel(node.SyntaxTree);
                var f = model.GetSymbolInfo(node);
                Console.WriteLine(f);
                if (DebuggerHelper.IsVerbose)
                {
                    Console.WriteLine("!!!!{0} : {1}", e.GetType().FullName, e.Message);
                    Console.WriteLine(node.CSharpKind() + " -> " + node.ToFullString());
                    Console.WriteLine(":::::::" + this.GetType().Name);

                }
            }

            return false;

        }

        private SymbolInfo GetSymbolInfo(ExpressionSyntax expression)
        {
            return this.SemanticModel.GetSymbolInfo(expression);
        }
        protected virtual bool Filter(SymbolInfo info)
        {
            return false;
        }


        public SemanticModel SemanticModel
        {
            get { return m_semanticModel; }
            set
            {
                if (null != m_semanticModel)
                {
                    Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + this.GetType());
                }
                m_semanticModel = value;
            }
        }

        public abstract string Walk(SyntaxNode node, SemanticModel model);

        [ImportMany(RequiredCreationPolicy = CreationPolicy.Shared)]
        public CustomObjectSyntaxWalker[] Walkers { get; set; }

        public CSharpCompilation Compilation { get; set; }
        public SyntaxTree[] Trees { get; set; }
        public CSharpSyntaxTree Snippet { get; set; }

        protected string EvaluateExpressionCode(ExpressionSyntax expression)
        {
            if (null == this.Walkers)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Walkers)
                throw new InvalidOperationException("Failed to load MEF");

            var symbol = GetSymbolInfo(expression);
            if (null != symbol.Symbol)
            {
                var w = GetWalker(symbol);
                if (null != w)
                    return w.Walk(expression, this.SemanticModel);

                Console.WriteLine("Cannot find symbol walker for " + expression.CSharpKind());
                Console.WriteLine("Name : " + symbol.Symbol.Name);
                Console.WriteLine("Display : " + symbol.Symbol.ToDisplayString());

            }
            var w2 = this.GetWalker(expression);
            if (null != w2)
                return w2.Walk(expression, this.SemanticModel);
            Console.WriteLine("No symbol could be produced for " + expression.CSharpKind());
            Console.WriteLine("expression : " + expression.ToFullString());
            return string.Empty;
        }


        protected CustomObjectSyntaxWalker GetWalker(SymbolInfo symbol, bool excludeThis = false)
        {
            var potentialWalkers = this.Walkers
                .Where(x => x.Filter(symbol))
                .ToList();
            if (excludeThis) potentialWalkers = this.Walkers
                .Where(x => x != this)
                 .Where(x => x.Filter(symbol))
                 .ToList();
            if (potentialWalkers.Count > 1)
            {
                potentialWalkers.ForEach(t => Console.WriteLine("{0} -> {1}", symbol.Symbol, t));
            }
            return potentialWalkers.FirstOrDefault();
        }

        protected CustomObjectSyntaxWalker GetWalker(SyntaxNode node, bool excludeThis = false)
        {
            var potentialWalkers = this.Walkers
                .Where(x => x.Filter(node))
                .ToList();
            if (excludeThis) potentialWalkers = this.Walkers
                 .Where(x => x != this)
                 .Where(x => x.Filter(node))
                 .ToList();
            if (potentialWalkers.Count > 1)
            {
                Console.WriteLine("!!!!! There are more that 1 walker for : " + node.CSharpKind());
                potentialWalkers.ForEach(t => Console.WriteLine("{0} -> {1}", node.CSharpKind(), t));
            }
            if (potentialWalkers.Count == 0)
                Console.WriteLine("!!!!! There is no walker for : " + node.CSharpKind());

            return potentialWalkers.FirstOrDefault();
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
            var walkers = this.Walkers.Where(x => x.Filter(node)).ToList();
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
