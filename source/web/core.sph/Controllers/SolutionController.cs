using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    [Authorize(Roles = "developers")]
    public class SolutionController : Controller
    {
        [Route("diagnostics")]
        public async Task<ActionResult> StartDiagnostics()
        {
            var context = new SphDataContext();

            // validate all entities
            // - record name
            // - security
            // - icon/image
            // - Operation, name, security, Setter Path
            var entitiesLo = await context.LoadAsync(context.EntityDefinitions, includeTotalRows: true);
            var entities = entitiesLo.ItemCollection;
            while (entitiesLo.HasNextPage)
            {
                entitiesLo =
                    await
                        context.LoadAsync(context.EntityDefinitions, entitiesLo.CurrentPage + 1, includeTotalRows: true);
                entities.AddRange(entitiesLo.ItemCollection);
            }

            var entitiesDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();
 
            Func<EntityDefinition, Task> er = async (f) =>
            {
                var br = await f.ValidateBuildAsync();
                br.Uri = $"entity.details/{f.Id}";
                entitiesDiagnostics.TryAdd(f.Name, br);
            };
            var tasks0 = entities.Select(x => er(x));
            await Task.WhenAll(tasks0);


            // validate all forms
            // - path
            // - route
            var formsLo = await context.LoadAsync(context.EntityForms, includeTotalRows: true);
            var forms = formsLo.ItemCollection;
            while (formsLo.HasNextPage)
            {
                formsLo = await context.LoadAsync(context.EntityForms, formsLo.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(formsLo.ItemCollection);
            }


            var formsDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();

            Func<EntityForm, Task> fr = async (f) =>
             {
                 var t = entities.SingleOrDefault(x => x.Id == f.EntityDefinitionId);
                 var result = await f.ValidateBuildAsync(t);
                 result.Uri = $"entity.form.designer/{f.EntityDefinitionId}/{f.Id}";
                 formsDiagnostics.TryAdd(f.Name, result);

             };
            var tasks = forms.Select(x => fr(x));
            await Task.WhenAll(tasks);


            // validate all views
            // - column path
            // - link , is correct route
            // - icon
            // - some suggestion to spelling
            // - route

            var viewsLo = await context.LoadAsync(context.EntityViews, includeTotalRows: true);
            var views = viewsLo.ItemCollection;
            while (viewsLo.HasNextPage)
            {
                viewsLo = await context.LoadAsync(context.EntityViews, viewsLo.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewsLo.ItemCollection);
            }


            var viewsDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();

            Func<EntityView, Task> vr = async (f) =>
            {
                var t = entities.SingleOrDefault(x => x.Id == f.EntityDefinitionId);
                var result = await f.ValidateBuildAsync(t);
                result.Uri = $"entity.view.designer/{f.EntityDefinitionId}/{f.Id}";
                viewsDiagnostics.TryAdd(f.Name, result);

            };
            var tasks2 = views.Select(vr);
            await Task.WhenAll(tasks2);


            // triggers
            // -> action
            // - filter
            // - compilation
            // - deployment
            // - active/inactive

            // workflow

            // 

            return Json(new { formsDiagnostics,  entitiesDiagnostics, viewsDiagnostics });
        }
    }
}