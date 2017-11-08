using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{

    public static class FilterExtension
    {
        public static bool IsMustNotFilter(this Filter filter, Entity item = null)
        {
            return !IsMustFilter(filter, item);
        }

        public static bool IsMustFilter(this Filter filter, Entity item = null)
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


        public static string GenerateQueryDsl(this Entity entity, Filter[] filters, Sort[] sorts = null,
            int skip = 0, int size = 20)
        {

            var elements = new Dictionary<string, string>();
            if (null != filters && filters.Any())
                elements.Add("filter", entity.GenerateBoolQueryDsl(filters));
            if (null != sorts && sorts.Any())
                elements.Add("sort", GenerateSorts(sorts));

            elements.Add("from", skip.ToString());
            elements.Add("size", size.ToString());

            return $@"{{
    {elements.ToString(",\r\n", x => $@"""{x.Key}"":{x.Value}")}

}}";
        }

        private static string GenerateSorts(this Sort[] sorts)
        {
            return $"[{sorts.ToString(",\r\n", x => x.GenerateQuery())}]";
        }

        public static string GenerateQuery(this Sort sort)
        {
            var direction = sort.Direction == SortDirection.Asc ? "asc" : "desc";
            return $@"{{""{sort.Path}"" : {{""order"": ""{direction}""}}}}";
        }


        public static string GenerateBoolQueryDsl(this Entity entity, IEnumerable<Filter> filters)
        {
            var filterList = (filters ?? Array.Empty<Filter>()).ToArray();

            var musts = filterList.Where(x => x.IsMustFilter(entity))
                .Select(x => x.GetFilterDsl(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");


            var mustNots = filterList.Where(x => x.IsMustNotFilter(entity))
                .Select(x => x.GetFilterDsl(entity, filterList))
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToString(",\r\n");


            // TODO : sorts , skip and size
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

        public static string GetFilterDsl(this Filter target, Entity entity, Filter[] filters)
        {
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
                default: throw new Exception(target.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}
