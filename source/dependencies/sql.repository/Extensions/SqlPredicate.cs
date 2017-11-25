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
                case Operator.IsNull:
                    if (target.Field.GetValue(context) is bool)
                    {
                        query.AppendLine($@"
                            ""missing"" : {{ ""field"" : ""{target.Term}""}}
                            ");
                    }
                    break;
                case Operator.FullText:
                    var field = target.Term == "*" ? "_all" : target.Term;
                    query.AppendLine($@"
                            ""query_string"" : {{ ""default_field"" : ""{field}"",""query"" : ""{
                            target.Field.GetValue(context)
                        }""}}
                            ");

                    break;
                case Operator.NotStartsWith:
                case Operator.StartsWith:
                    query.AppendLine($@"[{target.Term}] LIKE '{target.Field.GetValue(context)}%'");
                    break;
                case Operator.NotEndsWith:
                case Operator.EndsWith:
                    query.AppendLine($@"[{target.Term}] LIKE '%{target.Field.GetValue(context)}'");
                    break;
                default: throw new Exception(target.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}