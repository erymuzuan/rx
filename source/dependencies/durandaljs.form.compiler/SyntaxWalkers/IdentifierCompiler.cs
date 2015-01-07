using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
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

            return Walkers
                .Where(x => x.Filter(expression))
                .Select(x => x.Walk(expression))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        public virtual string Compile(SyntaxNode node, IEnumerable<ExpressionSyntax> arguments )
        {
            return string.Empty;
        }
    }
}