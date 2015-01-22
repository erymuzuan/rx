using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bespoke.Sph.Domain.QueryProviders {

    /// <summary>
    /// A ProjectionRow is an abstract over a row based data source
    /// </summary>
    public abstract class ProjectionRow {
        public abstract object GetValue(int index);
        public abstract IEnumerable<TE> ExecuteSubQuery<TE>(LambdaExpression query);
    }

    /// <summary>
    /// ProjectionBuilder is a visitor that converts an projector expression
    /// that constructs result objects out of ColumnExpressions into an actual
    /// LambdaExpression that constructs result objects out of accessing fields
    /// of a ProjectionRow
    /// </summary>
    public class ProjectionBuilder : DbExpressionVisitor {
        ParameterExpression m_row;
        string m_rowAlias;
        static MethodInfo m_miGetValue;
        static MethodInfo m_miExecuteSubQuery;
        
        public ProjectionBuilder() {
            if (m_miGetValue == null) {
                m_miGetValue = typeof(ProjectionRow).GetMethod("GetValue");
                m_miExecuteSubQuery = typeof(ProjectionRow).GetMethod("ExecuteSubQuery");
            }
        }

        public LambdaExpression Build(Expression expression, string alias) {
            this.m_row = Expression.Parameter(typeof(ProjectionRow), "row");
            this.m_rowAlias = alias;
            Expression body = this.Visit(expression);
            return Expression.Lambda(body, this.m_row);
        }

        protected override Expression VisitColumn(ColumnExpression column) {
            if (column.Alias == this.m_rowAlias) {
                return Expression.Convert(Expression.Call(this.m_row, m_miGetValue, Expression.Constant(column.Ordinal)), column.Type);
            }
            return column;
        }

        protected override Expression VisitProjection(ProjectionExpression proj) {
            LambdaExpression subQuery = Expression.Lambda(base.VisitProjection(proj), this.m_row);
            Type elementType = TypeSystem.GetElementType(subQuery.Body.Type);
            MethodInfo mi = m_miExecuteSubQuery.MakeGenericMethod(elementType);
            return Expression.Convert(
                Expression.Call(this.m_row, mi, Expression.Constant(subQuery)),
                proj.Type
                );
        }
    }


    /// <summary>
    /// ProjectionReader is an implemention of IEnumerable that converts data from DbDataReader into
    /// objects via a projector function,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectionReader<T> : IEnumerable<T>, IEnumerable {
        Enumerator m_enumerator;

        public ProjectionReader(/*DbDataReader reader, */Func<ProjectionRow, T> projector, IQueryProvider provider) {
            this.m_enumerator = new Enumerator(/*reader,*/ projector, provider);
        }

        public IEnumerator<T> GetEnumerator() {
            Enumerator e = this.m_enumerator;
            if (e == null) {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }
            this.m_enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        class Enumerator : ProjectionRow, IEnumerator<T> {
            //DbDataReader reader;
#pragma warning disable 649
            T current;
#pragma warning restore 649
            Func<ProjectionRow, T> m_projector;
            readonly IQueryProvider m_provider;

            public Enumerator(/*DbDataReader reader,*/ Func<ProjectionRow, T> projector, IQueryProvider provider) {
                //this.reader = reader;
                this.m_projector = projector;
                this.m_provider = provider;
            }

            public override object GetValue(int index) {
                //if (index >= 0) {
                //    if (this.reader.IsDBNull(index)) {
                //        return null;
                //    }
                //    else {
                //        return this.reader.GetValue(index);
                //    }
                //}
                throw new IndexOutOfRangeException();
            }

            public override IEnumerable<TE> ExecuteSubQuery<TE>(LambdaExpression query) {
                var projection = (ProjectionExpression) new Replacer().Replace(query.Body, query.Parameters[0], Expression.Constant(this));
                projection = (ProjectionExpression) Evaluator.PartialEval(projection, CanEvaluateLocally);
                var result = (IEnumerable<TE>)this.m_provider.Execute(projection);
                var list = new List<TE>(result);
                if (typeof(IQueryable<TE>).IsAssignableFrom(query.Body.Type)) {
                    return list.AsQueryable();
                }
                return list;
            }

            private static bool CanEvaluateLocally(Expression expression) {
                if (expression.NodeType == ExpressionType.Parameter ||
                    expression.NodeType.IsDbExpression()) {
                    return false;
                }
                return true;
            }

            public T Current
            {
                get { return this.current; }
            }

            object IEnumerator.Current
            {
                get { return this.current; }
            }

            public bool MoveNext() {
                //if (this.reader.Read())
                //{
                //    var xml = XElement.Parse(reader.GetString(0));
                //    XNamespace x = "http://www.bespoke.com.my/";
                //    Console.WriteLine(xml.Attribute("OrderNo"));
                //    this.current = this.projector(this);
                //    return true;
                //}
                return false;
            }

            public void Reset() {
            }

            public void Dispose() {
               // this.reader.Dispose();
            }
        }
    }
}
