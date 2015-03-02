using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.OdataQueryCompilers
{
    [Export]
    public class OdataQueryExpressionCompiler
    {

        [ImportMany(typeof(OdataSyntaxWalker), RequiredCreationPolicy = CreationPolicy.Shared, AllowRecomposition = true)]
        public OdataSyntaxWalker[] MefWalkers { get; set; }

        protected OdataSyntaxWalker[] Walkers
        {
            get
            {
                if (null == this.MefWalkers)
                    ObjectBuilder.ComposeMefCatalog(this);
                if (null == this.MefWalkers)
                    throw new InvalidOperationException("Cannot import MEF");
                return this.MefWalkers
                    .Distinct(new OdataSyntaxWalker.Comparer())
                    .ToArray();

            }
        }



        public string CompileExpression(SyntaxNode statement, SemanticModel model)
        {
            this.Walkers.ToList().ForEach(x => x.SemanticModel = model);
            var walkers = this.Walkers
                .Where(x => x.Filter(statement))
                .ToList();

            if (walkers.Count > 1)
            {
                Console.WriteLine("!!! " + statement.Kind());
                foreach (var w in walkers)
                {
                    Console.WriteLine(statement + " -> " + w.GetType().Name);
                }
            }

            var wk = walkers.LastOrDefault();
            if (null != wk)
                return wk.Walk(statement, model);
            return "!!! Cannot find Walker for : " + statement.Kind() + " -> " + statement.ToFullString();
        }
    }
}