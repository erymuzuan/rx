using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class Filter : DomainObject
    {
        public static string GenerateElasticSearchFilterDsl(Entity entity, IEnumerable<Filter> filterCollection)
        {
            var list = new ObjectCollection<Filter>(filterCollection);
            var fields = list.Select(f => f.Term).Distinct().ToArray();

            var query = new StringBuilder();

            var mustFilters = fields.Select(f => Filter.GetFilterDsl(entity, list.Where(x => x.Term == f && x.Operator != Operator.Neq).ToArray())).ToList();
            var musts = string.Join(",\r\n", mustFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());

            var mustNotFilters = fields.Select(f => Filter.GetFilterDsl(entity, list.Where(x => x.Term == f && x.Operator == Operator.Neq).ToArray())).ToList();
            var mustnots = string.Join(",\r\n", mustNotFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
            query.AppendFormat(@"{{
               ""bool"": {{
                  ""must"": [
                    {0}
                  ],
                  ""must_not"": [
                    {1}
                  ]
               }}
           }}", musts, mustnots);

            return query.ToString();
        }

        public static string GetFilterDsl(Entity entity, Filter[] filters)
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
                    if (val is string)
                        valJson = $"\"{val}\"";
                    if (val is DateTime)
                        valJson = $"\"{val:s}\"";
                    query.AppendLinf("                         \"{0}\":{1}", ft.Term, valJson);
                    query.AppendLine("                     }");
                    break;
                case Operator.Ge:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Lt:
                    query.AppendLine("                     \"range\":{");
                    query.AppendFormat("                         \"{0}\":{{", ft.Term);
                    var count = 0;
                    foreach (var t in filters)
                    {
                        count++;
                        var ov = $"{t.Field.GetValue(context)}";
                        DateTime dv;
                        if (DateTime.TryParse(ov, out dv))
                            ov = $"\"{dv:O}\"";

                        if (t.Operator == Operator.Ge || t.Operator == Operator.Gt)
                            query.AppendFormat("\"from\":{0}", ov);
                        if (t.Operator == Operator.Le || t.Operator == Operator.Lt)
                            query.AppendFormat("\"to\":{0}", ov);

                        if (count < filters.Length)
                            query.Append(",");

                    }
                    query.AppendLine("}");
                    query.AppendLine("                     }");
                    break;
                default: throw new Exception(ft.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}