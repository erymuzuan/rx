using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class IdentifierCompiler : ExportAttribute
    {
        [ImportMany(RequiredCreationPolicy = CreationPolicy.Shared)]
        public CustomObjectSyntaxWalker[] Walkers { get; set; }

        protected string EvaluateExpressionCode(ExpressionSyntax expression)
        {
            if (null == this.Walkers)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Walkers)
                throw new InvalidOperationException("Failed to load MEF");


            if (string.IsNullOrWhiteSpace(expression.ToString()))
                throw new InvalidOperationException("Just a marker");
            var walker = this.GetWalker(expression);
            if (null != walker) return walker.Walk(expression, null);

            throw new Exception("Now walker found for " + expression.Kind());
        }


        protected CustomObjectSyntaxWalker GetWalker(SyntaxNode node)
        {
            var potentialWalkers = this.Walkers
                .Where(x => x.Filter(node))
                .ToList();
            if (potentialWalkers.Count > 1)
            {
                Console.WriteLine("!!!!! There are more that 1 walker for : " + node.Kind());
                potentialWalkers.ForEach(t => Console.WriteLine("{0} -> {1}", node.Kind(), t));
            }
            if (potentialWalkers.Count == 0)
                Console.WriteLine("!!!!! There is no walker for : " + node.Kind());

            return potentialWalkers.FirstOrDefault();
        }

        public virtual string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments)
        {
            return string.Empty;
        }
        public virtual string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments, IEnumerable<TypeSyntax> genericTypeArguments )
        {
            return string.Empty;
        }
    }
}