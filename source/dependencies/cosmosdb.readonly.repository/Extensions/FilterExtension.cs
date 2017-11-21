using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class FilterExtension
    {
        public static bool? IsMustNotFilter(this Filter filter, Entity item = null)
        {
            return !IsMustFilter(filter, item);
        }

        public static bool? IsMustFilter(this Filter filter, Entity item = null)
        {
            var rc = new RuleContext(item);
            switch (filter.Operator)
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
                    if (filter.Field.GetValue(rc) is bool cf)
                    {
                        return cf;
                    }
                    break;
                case Operator.IsNotNull:
                    if (filter.Field.GetValue(rc) is bool cb)
                    {
                        return !cb;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }



        public static string CompileToElasticsearchBoolQuery<T>(this IEnumerable<Filter> filters) where T : Entity, new()
        {
            return new T().CompileToElasticsearchBoolQuery(filters);
        }

        public static string CompileToElasticsearchFullTextQuery(this Entity entity, IEnumerable<Filter> filters)
        {
            var filterList = (filters ?? Array.Empty<Filter>()).ToArray();
            if(filterList.Count(x => x.Operator == Operator.FullText) > 1)
                throw new ArgumentException(@"You cannot have more than 1 FullText operator specifed in a query", nameof(filters));

            var queries = filterList.Where(x => !x.IsMustFilter(entity).HasValue)
                .Select(x => x.CompileToElasticsearchTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");

            return queries;

        }
        public static string CompileToElasticsearchBoolQuery(this Entity entity, IEnumerable<Filter> filters)
        {
            var filterList = (filters ?? Array.Empty<Filter>()).ToArray();

            var musts = filterList.Where(x => x.IsMustFilter(entity) ?? false)
                .Select(x => x.CompileToElasticsearchTermLevelQuery(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");


            var mustNots = filterList.Where(x => x.IsMustNotFilter(entity) ?? false)
                .Select(x => x.CompileToElasticsearchTermLevelQuery(entity, filterList))
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

        public static string CompileToElasticsearchTermLevelQuery<T>(this Filter target, Filter[] filters = null) where T : Entity, new()
        {
            return target.CompileToElasticsearchTermLevelQuery(new T());
        }

        private static string CompileToElasticsearchTermLevelQuery(this Filter target, Entity entity, Filter[] filters = null)
        {
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
                            ""query_string"" : {{ ""default_field"" : ""{field}"",""query"" : ""{target.Field.GetValue(context)}""}}
                            ");

                    break;

                default: throw new Exception(target.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}
