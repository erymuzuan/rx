using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using Bespoke.Sph.Domain.QueryProviders;

namespace Bespoke.Sph.OdataRepository
{

    /// <summary>
    /// A LINQ query provider that executes SQL queries over a DbConnection
    /// </summary>
    public class OdataQueryProvider : QueryProvider
    {

        TextWriter m_log;


        public TextWriter Log
        {
            get { return this.m_log; }
            set { this.m_log = value; }
        }

        public override string GetQueryText(Expression expression)
        {
            return this.Translate(expression).CommandText;
        }

        public override object Execute(Expression expression)
        {
            return this.Execute(this.Translate(expression));
        }

        private object Execute(TranslateResult query)
        {
            
            query.Projector.Compile();

            if (this.m_log != null)
            {
                this.m_log.WriteLine(query.CommandText);
                this.m_log.WriteLine();
            }

            var text = query.CommandText;
            Debug.WriteLine(text);
            var list = new List<object>();
           
            return list;
        }

        internal class TranslateResult
        {
            internal string CommandText;
            internal LambdaExpression Projector;
        }

        private TranslateResult Translate(Expression expression)
        {
            var projection = expression as ProjectionExpression;
            if (projection == null)
            {
                expression = Evaluator.PartialEval(expression, CanBeEvaluatedLocally);
                expression = new QueryBinder(this).Bind(expression);
                expression = new OrderByRewriter().Rewrite(expression);
                expression = new UnusedColumnRemover().Remove(expression);
                expression = new RedundantSubqueryRemover().Remove(expression);
                projection = (ProjectionExpression)expression;
            }
            string commandText = new OdataQueryFormatter().Format(projection.Source);
            LambdaExpression projector = new ProjectionBuilder().Build(projection.Projector, projection.Source.Alias);
            return new TranslateResult { CommandText = commandText, Projector = projector };
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter &&
                   expression.NodeType != ExpressionType.Lambda;
        }
    }
}
