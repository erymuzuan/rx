using System.Linq;
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
                                   $"[Filter] : {f.Term} => '{f.Field}' does not have term or field"
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
                                 $"[Sort] : {f.Path} does not have path"
                                 );
            var columnErrors = from f in this.ViewColumnCollection
                               where string.IsNullOrWhiteSpace(f.Path)
                               select new BuildError
                               (
                                   this.WebId,
                                   string.Format("[Column] : {1}({0}) does not have path", f.Path, f.Header)
                               );
            var linkErrors = from f in this.ViewColumnCollection
                             where string.IsNullOrWhiteSpace(f.FormRoute)
                             && f.IsLinkColumn
                             select new BuildError
                             (
                                 this.WebId,
                                 string.Format("[Column] : {1}({0})  does not have for route", f.Path, f.Header)
                             );

            var paths = ed.GetMembersPath();
            var invalidPathWarnings = from f in this.ViewColumnCollection
                                      where !paths.Contains(f.Path)
                                      select new BuildError(f.WebId, $"[{f.Header}] : Specified path is \"{f.Path}\" may not be valid, ignore this warning if this is intentional");
            result.Warnings.AddRange(invalidPathWarnings);

            var invalidFilters = from f in this.FilterCollection
                                      where !paths.Contains(f.Term)
                                      select new BuildError(f.WebId, $"[{f.Term}] : Specified filter term is \"{f.Term}\" may not be valid");
            result.Errors.AddRange(invalidFilters);

            var routes = (await JsRoute.GetCustomRoutes()).Select(x => x.Route).ToArray();
            var invalidLinks = from f in this.ViewColumnCollection
                               where f.IsLinkColumn
                               && !routes.Contains(f.FormRoute)
                               select new BuildError(f.WebId, $"[{f.Header}] : Specified link route in column \"{f.Header}\" point to \"{f.FormRoute}\" is invalid");
            result.Errors.AddRange(invalidLinks);

            // TODO  validate for route parameters



            if (string.IsNullOrWhiteSpace(this.Route))
                result.Errors.Add(new BuildError(this.WebId, "Route is missing"));
            if (!this.Performer.Validate())
                result.Errors.Add(new BuildError(this.WebId, "You have not set the permission correctly"));


            var context = new SphDataContext();

            var formRouteCountTask = context.GetCountAsync<EntityForm>(f => f.Route == this.Route);
            var viewRouteCountTask = context.GetCountAsync<EntityView>(f => f.Route == this.Route && f.Id != this.Id);
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
                    select $"'{s.CssClass}':{s.Condition}";
            return "css : {" + string.Join(",\r\n", f.ToArray()) + "}";
        }

        public string GenerateEsSortDsl()
        {
            var f = from s in this.SortCollection
                    select $"{{\"{s.Path}\":{{\"order\":\"{s.Direction.ToString().ToLowerInvariant()}\"}}}}";
            return "[" + string.Join(",\r\n", f.ToArray()) + "]";
        }

        public void AddFilter(string term, Operator @operator, Field field)
        {
            this.FilterCollection.Add(new Filter { Field = field, Operator = @operator, Term = term });
        }

 


        public override string ToString()
        {
            return $"[{this.Id}] {this.Name}";
        }

    

        public string GenerateRoute()
        {
            if (!this.RouteParameterCollection.Any())
                return $"{this.Route.ToLowerInvariant()}";
            return $"{this.Route.ToLowerInvariant()}"
                   + "/:" + string.Join("/:", this.RouteParameterCollection.Select(r => r.Name));
        }
    }
}