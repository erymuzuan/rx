using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public class RouteDiagnostics : BuilDiagnostic
    {
        private string RoutePattern = @"^[a-z0-9-._]*$";
        private string NamePattern = @"^[A-Za-z][A-Za-z0-9 -]*$";

        public override async Task<BuildError[]> ValidateErrorsAsync(EntityView view, EntityDefinition entity)
        {

            var errors = new List<BuildError>();

            if (string.IsNullOrWhiteSpace(view.Route))
                errors.Add(new BuildError(view.WebId, "Route is missing"));
            var context = new SphDataContext();

            var formRouteCountTask = context.GetCountAsync<EntityForm>(f => f.Route == view.Route);
            var viewRouteCountTask = context.GetCountAsync<EntityView>(f => f.Route == view.Route && f.Id != view.Id);
            var entityRouteCountTask = context.GetCountAsync<EntityDefinition>(f => f.Name == view.Route);

            await Task.WhenAll(formRouteCountTask, viewRouteCountTask, entityRouteCountTask).ConfigureAwait(false);

            if (await formRouteCountTask > 0)
                errors.Add(new BuildError(view.WebId, "The route is already in used by a form"));

            if (await viewRouteCountTask > 0)
                errors.Add(new BuildError(view.WebId, "The route is already in used by another view"));

            if (await entityRouteCountTask > 0)
                errors.Add(new BuildError(view.WebId, "The route is already in used, cannot be the same as an entity name"));


            if (!view.ViewColumnCollection.Any())
                errors.Add(new BuildError(view.WebId, "Your view`s are missing columns"));

            var validName = new Regex(NamePattern);
            if (!validName.Match(view.Name).Success)
                errors.Add(new BuildError(view.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            var validRoute = new Regex(RoutePattern);
            if (!validRoute.Match(view.Route).Success)
                errors.Add(new BuildError(view.WebId) { Message = "Route must be lower case.You cannot use symbol or number as first character, or other chars except _ - ." });


            
            return errors.ToArray();
        }

        public override async Task<BuildError[]> ValidateErrorsAsync(EntityForm form, EntityDefinition entity)
        {
            var result = new BuildValidationResult();
            var context = new SphDataContext();

            var formRouteCountTask = context.GetCountAsync<EntityForm>(f => f.Route == form.Route && f.Id != form.Id);
            var viewRouteCountTask = context.GetCountAsync<EntityView>(f => f.Route == form.Route);
            var entityRouteCountTask = context.GetCountAsync<EntityDefinition>(f => f.Name == form.Route);

            await Task.WhenAll(formRouteCountTask, viewRouteCountTask, entityRouteCountTask).ConfigureAwait(false);

            if (await formRouteCountTask > 0)
                result.Errors.Add(new BuildError(form.WebId, "The route is already in used by another form"));

            if (await viewRouteCountTask > 0)
                result.Errors.Add(new BuildError(form.WebId, "The route is already in used by a view"));

            if (await entityRouteCountTask > 0)
                result.Errors.Add(new BuildError(form.WebId, "The route is already in used, cannot be the same as an entity name"));

            var validName = new Regex(NamePattern);
            if (!validName.Match(form.Name).Success)
                result.Errors.Add(new BuildError(form.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });

            var validRoute = new Regex(RoutePattern);
            if (!validRoute.Match(form.Route).Success)
                result.Errors.Add(new BuildError(form.WebId) { Message = "Route must be lower case.You cannot use symbol or number as first character, or other chars except _ - ." });

            return result.Errors.ToArray();

        }


    }
}