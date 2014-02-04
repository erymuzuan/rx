using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class EntityView : Entity
    {
        public string GenerateEsSortDsl()
        {
            var f = from s in this.SortCollection
                    select string.Format("{{\"{0}\":{{\"order\":\"{1}\"}}}}", s.Path, s.Direction.ToString().ToLowerInvariant());
            return "[" + string.Join(",\r\n", f.ToArray()) + "]";
        }

        public void AddFilter(string term, Operator @operator, Field field)
        {
            this.FilterCollection.Add(new Filter { Field = field, Operator = @operator, Term = term });
        }

        public string GenerateElasticSearchFilterDsl()
        {
            var list = new ObjectCollection<Filter>(this.FilterCollection);
            var fields = this.FilterCollection.Select(f => f.Term).Distinct().ToArray();

            var query = new StringBuilder();
            var filters = from f in fields
                          select this.GetFilterDsl(list.Where(x => x.Term == f).ToArray());
            query.AppendFormat(@"{{
               ""and"": {{
                  ""filters"": [
                    {0}
                  ]
               }}
           }}", string.Join(",\r\n", filters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()));

            return query.ToString();
        }


        public string GetFilterDsl(Filter[] filters)
        {
            var ft = filters.First();
            var query = new StringBuilder();
            query.AppendLine("                 {");
            if (ft.Operator == Operator.Eq)
            {
                query.AppendLine("                     \"term\":{");
                query.AppendLinf("                         \"{0}\":\"{1}\"", ft.Term, ft.Field.GetValue(null));
                query.AppendLine("                     }");
            }
            else if (ft.Operator == Operator.Ge || ft.Operator == Operator.Gt ||
                ft.Operator == Operator.Le || ft.Operator == Operator.Lt)
            {

                query.AppendLine("                     \"range\":{");
                query.AppendFormat("                         \"{0}\":{{", ft.Term);
                var count = 0;
                foreach (var t in filters)
                {
                    count++;
                    if (t.Operator == Operator.Ge)
                        query.AppendFormat("\"from\":{0}",t.Field.GetValue(null));
                    if (t.Operator == Operator.Le)
                        query.AppendFormat("\"to\":{0}", t.Field.GetValue(null));

                    if (count < filters.Length)
                        query.Append(",");

                }
                query.AppendLine("}");
                query.AppendLine("                     }");
            }
            else
            {
                return string.Empty;
            }



            query.AppendLine("                 }");

            return query.ToString();
        }
    }
}