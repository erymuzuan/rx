using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class EntityView : Entity
    {
        public async Task<BuildValidationResult> ValidateBuild(EntityDefinition ed)
        {
            var result = new BuildValidationResult();
            var filterErrors = from f in this.FilterCollection
                               where string.IsNullOrWhiteSpace(f.Term) || null == f.Field
                               select new BuildError
                               (
                                   this.WebId,
                                   string.Format("[Filter] : {0} => '{1}' does not have term or field", f.Term, f.Field)
                               );
            var sortErrors = from f in this.SortCollection
                             where string.IsNullOrWhiteSpace(f.Path)
                             select new BuildError
                             (
                                 this.WebId,
                                 string.Format("[Sort] : {0} does not have path", f.Path)
                             );
            var columnErrors = from f in this.ViewColumnCollection
                             where string.IsNullOrWhiteSpace(f.Path)
                             select new BuildError
                             (
                                 this.WebId,
                                 string.Format("[Column] : {0} does not have path", f.Path)
                             );
            if (string.IsNullOrWhiteSpace(this.Route))
                result.Errors.Add(new BuildError(this.WebId, "Route is missing"));


            var context = new SphDataContext();
            var count = await context.GetCountAsync<EntityForm>(f => f.Route == this.Route);
            if (count > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used"));
            var count2 = await context.GetCountAsync<EntityView>(f => f.Route == this.Route && f.EntityViewId != this.EntityViewId);
            if (count2 > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used"));

            if(!this.ViewColumnCollection.Any())
                result.Errors.Add(new BuildError(this.WebId, "Your views are missing columns"));

            result.Errors.AddRange(columnErrors);
            result.Errors.AddRange(filterErrors);
            result.Errors.AddRange(sortErrors);
            result.Result = result.Errors.Count == 0;

            return result;
        }


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
                        query.AppendFormat("\"from\":{0}", t.Field.GetValue(null));
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