using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public class SqlPredicate
    {
        public Filter[] Filters { get; }
        public Filter Filter { get; }

        public SqlPredicate(Filter filter)
        {
            Filter = filter;
        }
        public SqlPredicate(IEnumerable<Filter> filters)
        {
            Filters = filters.ToArray();
        }

        public string CompileToBoolQuery<T>()
            where T : DomainObject, new()
        {
            return this.CompileToBoolQuery(new T());
        }

        public string CompileToFullTextQuery(DomainObject entity)
        {
            var filterList = (this.Filters ?? Array.Empty<Filter>()).ToArray();
            if (filterList.Count(x => x.Operator == Operator.FullText) > 1)
                throw new ArgumentException(@"You cannot have more than 1 FullText operator specifed in a query",
                    nameof(this.Filters));

            var queries = filterList.Select(x => x.ToSqlPredicate())
                .Select(x => x.CompileToTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");

            return queries;
        }

        public string CompileToBoolQuery(DomainObject entity)
        {
            var filterList = (this.Filters ?? Array.Empty<Filter>()).ToArray();

            var predicates = filterList.Select(x => x.ToSqlPredicate())
                .Select(x => x.CompileToTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString("\r\nAND\r\n");



            return predicates;
        }

        public string CompileToTermLevelQuery<T>(Filter[] filters = null)
            where T : Entity, new()
        {
            var target = this.Filter.ToSqlPredicate();
            return target.CompileToTermLevelQuery(new T());
        }

        private string CompileToTermLevelQuery(DomainObject entity, Filter[] filters = null)
        {
            var target = this.Filter;
            if (null == filters)
                filters = new[] { target };

            var context = new RuleContext(entity);
            var query = new StringBuilder();
            query.AppendLine("                 {");

            switch (target.Operator)
            {
                case Operator.Eq:
                case Operator.Neq:
                case Operator.Ge:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Lt:
                    var val = target.Field.GetValue(context);
                    var valJson = $"{val}";
                    switch (val)
                    {
                        case decimal _:
                            valJson = $"{val}";
                            break;
                        case int _:
                            valJson = $"{val}";
                            break;
                        case string _:
                            valJson = $"'{val}'";
                            break;
                        case DateTime _:
                            valJson = $"'{val:s}'";
                            break;
                    }
                    return $@"[{target.Term}] {target.Operator.ToSqlOperator()} {valJson}";
                case Operator.IsNotNull:
                    if (target.Field.GetValue(context) is bool bv2)
                    {
                        return bv2 ? $"[{target.Term}] IS NOT NULL" : $"[{target.Term}] IS NULL";
                    }
                    throw new InvalidOperationException("IsNull must be true or false");
                case Operator.IsNull:
                    if (target.Field.GetValue(context) is bool bv)
                    {
                        return bv ? $"[{target.Term}] IS NULL" : $"[{target.Term}] IS NOT NULL";
                    }
                    throw new InvalidOperationException("IsNull must be true or false");
                case Operator.FullText:
                    throw new NotImplementedException("Requires Sql Server FullText catalogue, enabled.. coming soon... or never");
                case Operator.NotStartsWith:
                    return $@"[{target.Term}] NOT LIKE '{target.Field.GetValue(context)}%'";
                case Operator.StartsWith:
                    return $@"[{target.Term}] LIKE '{target.Field.GetValue(context)}%'";
                case Operator.NotEndsWith:
                    return $@"[{target.Term}] NOT LIKE '%{target.Field.GetValue(context)}'";
                case Operator.EndsWith:
                    return $@"[{target.Term}] LIKE '%{target.Field.GetValue(context)}'";
                default: throw new Exception(target.Operator + " is not supported for filter DSL yet");
            }

        }
    }
}