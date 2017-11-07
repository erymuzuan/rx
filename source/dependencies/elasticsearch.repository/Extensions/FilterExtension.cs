using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class FilterExtension
    {
        public static bool IsMustFilter(this Filter filter, Entity item, string field)
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
                    return field == filter.Term;
                case Operator.NotContains:
                case Operator.Neq:
                case Operator.NotStartsWith:
                case Operator.NotEndsWith:
                    return filter.Term != field;
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

        public static string GenerateElasticSearchFilterDsl(this Entity entity, IEnumerable<Filter> filterCollection, int skip =0, int size = 20)
        {
            // TODO : skip and size
            var list = new ObjectCollection<Filter>(filterCollection);
            var fields = list.Select(f => f.Term).Distinct().ToArray();

            var query = new StringBuilder();

            var mustFilters = fields.Select(f =>
GetFilterDsl(entity, list.Where(x => x.IsMustFilter(entity, f)).ToArray())).ToList();
            var musts = string.Join(",\r\n", mustFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());

            var mustNotFilters = fields.Select(f =>
GetFilterDsl(entity, list.Where(x => !x.IsMustFilter(entity, f)).ToArray())).ToList();
            var mustNots = mustNotFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToString("\r\n");
            query.AppendFormat(@"{{
               ""bool"": {{
                  ""must"": [
                    {0}
                  ],
                  ""must_not"": [
                    {1}
                  ]
               }}
           }}", musts, mustNots);

            return query.ToString();
        }

        public static string GetFilterDsl(this Entity entity, Filter[] filters)
        {
            var context = new RuleContext(entity);
            var ft = filters.FirstOrDefault();
            if (null == ft) return null;
            var query = new StringBuilder();
            query.AppendLine("                 {");

            switch (ft.Operator)
            {
                case Operator.Eq:
                case Operator.Neq:
                    query.AppendLine("                     \"term\":{");
                    var val = ft.Field.GetValue(context);
                    var valJson = $"{val}";
                    switch (val)
                    {
                        case string _:
                            valJson = $"\"{val}\"";
                            break;
                        case DateTime _:
                            valJson = $"\"{val:s}\"";
                            break;
                    }
                    query.AppendLinf("                         \"{0}\":{1}", ft.Term, valJson);
                    query.AppendLine("                     }");
                    break;
                case Operator.Ge:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Lt:
                    query.AppendLine("                     \"range\":{");
                    query.Append($"                         \"{ft.Term}\":{{");
                    var count = 0;
                    foreach (var t in filters)
                    {
                        count++;
                        var ov = $"{t.Field.GetValue(context)}";
                        if (DateTime.TryParse(ov, out var dv))
                            ov = $"\"{dv:O}\"";

                        switch (t.Operator)
                        {
                            case Operator.Ge:
                            case Operator.Gt:
                                query.Append($"\"from\":{ov}");
                                break;
                            case Operator.Le:
                            case Operator.Lt:
                                query.Append($"\"to\":{ov}");
                                break;
                        }

                        if (count < filters.Length)
                            query.Append(",");
                    }
                    query.AppendLine("}");
                    query.AppendLine("                     }");
                    break;
                case Operator.IsNotNull:
                case Operator.IsNull:
                    if (ft.Field.GetValue(context) is bool)
                    {
                        query.AppendLine($@"
                            ""missing"" : {{ ""field"" : ""{ft.Term}""}}
                            ");
                    }
                    break;
                default: throw new Exception(ft.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}
