using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class EntityView : Entity
    {
        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            var result = new BuildValidationResult();
            var filterErrors = from f in this.FilterCollection
                               where string.IsNullOrWhiteSpace(f.Term) || null == f.Field
                               select new BuildError
                               (
                                   this.WebId,
                                   string.Format("[Filter] : {0} => '{1}' does not have term or field", f.Term, f.Field)
                               );
            var conditionalFormattingErrors = from f in this.ConditionalFormattingCollection
                             where string.IsNullOrWhiteSpace(f.Condition) || f.Condition.Contains("\"")
                             select new BuildError
                             (
                                 this.WebId,
                                 "[ConditionalFormatting] : Condition cannot contains \" or empty"
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
            var linkErrors = from f in this.ViewColumnCollection
                             where string.IsNullOrWhiteSpace(f.FormRoute)
                             && f.IsLinkColumn
                             select new BuildError
                             (
                                 this.WebId,
                                 string.Format("[Column] : {0} does not have for route", f.Path)
                             );
            if (string.IsNullOrWhiteSpace(this.Route))
                result.Errors.Add(new BuildError(this.WebId, "Route is missing"));


            var context = new SphDataContext();

            var formRouteCountTask = context.GetCountAsync<EntityForm>(f => f.Route == this.Route);
            var viewRouteCountTask = context.GetCountAsync<EntityView>(f => f.Route == this.Route && f.EntityViewId != this.EntityViewId);
            var entityRouteCountTask = context.GetCountAsync<EntityDefinition>(f => f.Name == this.Route);

            await Task.WhenAll(formRouteCountTask, viewRouteCountTask, entityRouteCountTask).ConfigureAwait(false);

            if (await formRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used by a form"));

            if (await viewRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used by another view"));

            if (await entityRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used, cannot be the same as an entity name"));


            if (!this.ViewColumnCollection.Any())
                result.Errors.Add(new BuildError(this.WebId, "Your views are missing columns"));

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9 -]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            var validRoute = new Regex(@"^[a-z0-9-._]*$");
            if (!validRoute.Match(this.Route).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Route must be lower case.You cannot use symbol or number as first character, or other chars except _ - ." });


            result.Errors.AddRange(linkErrors);
            result.Errors.AddRange(columnErrors);
            result.Errors.AddRange(filterErrors);
            result.Errors.AddRange(sortErrors);
            result.Errors.AddRange(conditionalFormattingErrors);
            result.Result = result.Errors.Count == 0;

            return result;
        }


        public string GenerateConditionalFormattingBinding()
        {
            if (!this.ConditionalFormattingCollection.Any())
                return string.Empty;
            var f = from s in this.ConditionalFormattingCollection
                    select string.Format("'{0}':{1}", s.CssClass, s.Condition);
            return "css : {" + string.Join(",\r\n", f.ToArray()) + "}";
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

            var mustFilters = fields.Select(f => this.GetFilterDsl(list.Where(x => x.Term == f && x.Operator != Operator.Neq).ToArray())).ToList();
            var musts = string.Join(",\r\n", mustFilters.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());

            var mustNotFilters = fields.Select(f => this.GetFilterDsl(list.Where(x => x.Term == f && x.Operator == Operator.Neq).ToArray())).ToList();
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


        public string GetFilterDsl(Filter[] filters)
        {
            var context = new RuleContext(this);
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
                    var valJson = string.Format("{0}", val);
                    if (val is string)
                        valJson = string.Format("\"{0}\"", val);
                    if (val is DateTime)
                        valJson = string.Format("\"{0:s}\"", val);
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
                        if (t.Operator == Operator.Ge)
                            query.AppendFormat("\"from\":{0}", t.Field.GetValue(context));
                        if (t.Operator == Operator.Le)
                            query.AppendFormat("\"to\":{0}", t.Field.GetValue(context));

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

        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.EntityViewId, this.Name);
        }

        public override void SetId(int id)
        {
            this.EntityViewId = id;
        }

        public override int GetId()
        {
            return this.EntityViewId;
        }
    }
}