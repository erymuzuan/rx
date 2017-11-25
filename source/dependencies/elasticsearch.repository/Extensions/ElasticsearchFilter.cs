using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public class ElasticsearchFilter
    {
        private readonly JObject m_mapping;
        public Filter[] Filters { get; }
        public Filter Filter { get; }

        public ElasticsearchFilter(Filter filter)
        {
            Filter = filter;
        }
        public ElasticsearchFilter(Filter filter, JObject mapping) : this(filter)
        {
            m_mapping = mapping;
        }
        public ElasticsearchFilter(IEnumerable<Filter> filters)
        {
            Filters = filters.ToArray();
        }
        public ElasticsearchFilter(IEnumerable<Filter> filters, JObject mapping) : this(filters)
        {
            m_mapping = mapping;
        }

        public bool? IsMustNotFilter(DomainObject item = null)
        {
            return !IsMustFilter(item);
        }

        public bool? IsMustFilter(DomainObject item = null)
        {
            var rc = new RuleContext(item);
            switch (this.Filter.Operator)
            {
                case Operator.Eq:
                case Operator.Lt:
                case Operator.Le:
                case Operator.Gt:
                case Operator.Ge:
                case Operator.Substringof:
                case Operator.StartsWith:
                case Operator.EndsWith:
                    return true;
                case Operator.FullText:
                    return default;
                case Operator.NotContains:
                case Operator.Neq:
                case Operator.NotStartsWith:
                case Operator.NotEndsWith:
                    return false;
                case Operator.IsNull:
                    if (this.Filter.Field.GetValue(rc) is bool cf)
                    {
                        return cf;
                    }
                    break;
                case Operator.IsNotNull:
                    if (this.Filter.Field.GetValue(rc) is bool cb)
                    {
                        return !cb;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
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

            var queries = filterList.Select(x => x.ToElasticsearchFilter(m_mapping)).Where(x => !x.IsMustFilter(entity).HasValue)
                .Select(x => x.CompileToTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");

            return queries;
        }

        public string CompileToBoolQuery(DomainObject entity)
        {
            var filterList = (this.Filters ?? Array.Empty<Filter>()).ToArray();

            var musts = filterList.Select(x => x.ToElasticsearchFilter(m_mapping)).Where(x => x.IsMustFilter(entity) ?? false)
                .Select(x => x.CompileToTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");


            var mustNots = filterList.Select(x => x.ToElasticsearchFilter(m_mapping)).Where(x => x.IsMustNotFilter(entity) ?? false)
                .Select(x => x.CompileToTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");

            return $@"{{
               ""bool"": {{
                  ""must"": [
                    {musts}
                  ],
                  ""must_not"": [
                    {mustNots}
                  ]
               }}
           }}";
        }

        public string CompileToTermLevelQuery<T>( Filter[] filters = null)
            where T : Entity, new()
        {
            var target = this.Filter.ToElasticsearchFilter(m_mapping);
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
                    query.AppendLine("                     \"term\":{");
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
                            valJson = $"\"{val}\"";
                            break;
                        case DateTime _:
                            valJson = $"\"{val:s}\"";
                            break;
                    }
                    query.AppendLinf("                         \"{0}\":{1}", target.Term, valJson);
                    query.AppendLine("                     }");
                    break;
                case Operator.Ge:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Lt:

                    var ranges = new Dictionary<string, object>();
                    query.AppendLine(@"                     ""range"":{");
                    query.Append($@"                         ""{target.Term}"":{{");
                    foreach (var t in filters.Where(x => x.Term == target.Term))
                    {
                        var ov = $"{t.Field.GetValue(context)}";
                        if (DateTime.TryParse(ov, out var dv))
                            ov = $"\"{dv:O}\"";

                        switch (t.Operator)
                        {
                            case Operator.Ge:
                                ranges.Add("gte", ov);
                                break;
                            case Operator.Gt:
                                ranges.Add("gt", ov);
                                break;
                            case Operator.Le:
                                ranges.Add("lte", ov);
                                break;
                            case Operator.Lt:
                                ranges.Add("lt", ov);
                                break;
                        }
                    }
                    query.AppendLine(ranges.ToString(",\r\n", x => $@"""{x.Key}"":{x.Value}"));
                    query.AppendLine("}");
                    query.AppendLine("                     }");
                    break;
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
                    var (_, analyzed) = this.m_mapping.GetMapping(target.Term);
                    if (analyzed != "not_analyzed")
                        throw new NotSupportedException($"{target.Term} is {analyzed} and not supported for prefix term query in Elasticsearch");
                    query.AppendLine($@"""prefix"": {{""{target.Term}"": ""{target.Field.GetValue(context)}"" }}");
                    break;
                case Operator.NotEndsWith:
                case Operator.EndsWith:
                    throw new NotSupportedException("Elasticsearch doesn't support EndsWith term query in filter, let me know if you find  a way to do it");

                default: throw new Exception(target.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}