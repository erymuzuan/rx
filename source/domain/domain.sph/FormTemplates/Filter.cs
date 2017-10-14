using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class Filter : DomainObject
    {
        public Filter()
        {
        }

        public Filter(string term, object value)
        {
            this.Term = term;
            this.Operator = Operator.Eq;
            this.Field = new ConstantField
            {
                Value = value as string,
                Type = typeof(string)
            };
        }

        public Filter(string term, Operator op, object value)
        {
            this.Term = term;
            this.Operator = op;
            this.Field = new ConstantField
            {
                Value = value as string,
                Type = typeof(string)
            };
        }

        public static Filter Parse(string json)
        {
            var jo = JObject.Parse(json);
            var term = jo.SelectToken("$.term").Value<string>();
            var op1 = jo.SelectToken("$.operator").Value<string>();
            var op = (Operator) Enum.Parse(typeof(Operator), op1);
            var valueNode = (JValue) jo.SelectToken("$.value").Value<object>();
            return new Filter(term, op, valueNode.Value);
        }

        public static Filter Parse(JToken jo)
        {
            var term = jo.SelectToken("$.term").Value<string>();
            var op1 = jo.SelectToken("$.operator").Value<string>();
            var op = (Operator) Enum.Parse(typeof(Operator), op1);
            var valueNode = (JValue) jo.SelectToken("$.value").Value<object>();
            return new Filter(term, op, valueNode.Value);
        }


        private bool IsMustFilter(Filter filter, string field)
        {
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
                    break;
                case Operator.Neq:
                    break;
                case Operator.NotStartsWith:
                    break;
                case Operator.NotEndsWith:
                    break;
                case Operator.IsNull:
                    if (filter.Field is ConstantField cf)
                    {
                        if (cf.Value is bool cfv)
                        {
                            if (cfv)
                            {
                                
                            }
                        }
                    }
                    break;
                case Operator.IsNotNull:
                    break;
            }
            return true;
        }

        public static string GenerateElasticSearchFilterDsl(Entity entity, IEnumerable<Filter> filterCollection)
        {
            var list = new ObjectCollection<Filter>(filterCollection);
            var fields = list.Select(f => f.Term).Distinct().ToArray();

            var query = new StringBuilder();

            var mustFilters = fields.Select(f =>
                GetFilterDsl(entity, list.Where(x => x.Term == f && x.Operator != Operator.Neq).ToArray())).ToList();
            var musts = string.Join(",\r\n", mustFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());

            var mustNotFilters = fields.Select(f =>
                GetFilterDsl(entity, list.Where(x => x.Term == f && x.Operator == Operator.Neq).ToArray())).ToList();
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
                    if (ft.Field.GetValue(context) is bool cb)
                    {
                        if (cb)
                        {
                            query.AppendLine($@"
                            ""missing"" : {{ ""field"" : ""{ft.Term}""}}
                            ");
                        }
                        else
                        {
                            query.AppendLine($@"
                            ""missing"" : {{ ""field"" : ""{ft.Term}""}}
                            ");
                        }
                    }
                    break;
                default: throw new Exception(ft.Operator + " is not supported for filter DSL yet");
            }


            query.AppendLine("                 }");

            return query.ToString();
        }

        public virtual async Task<IEnumerable<BuildError>> ValidateErrorsAsync()
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.Term))
                errors.Add(new BuildError(this.WebId, "Term cannot be empty"));
            if (null == this.Field)
                errors.Add(new BuildError(this.WebId, $"Filed cannot be null for {Term} filter"));

            if (null != this.Field)
            {
                var fieldErrors = await this.Field.ValidateErrorsAsync(this);
                errors.AddRange(fieldErrors);
            }

            return errors;
        }

        public virtual Task<IEnumerable<BuildError>> ValidateWarningsAsync()
        {
            return Task.FromResult(Array.Empty<BuildError>().AsEnumerable());
        }
    }
}