using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UserProfileController : BaseAppController
    {
        public ActionResult Html()
        {
            return View();
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js()
        {
            var context = new SphDataContext();
            var user = Membership.GetUser();
            var profile = await context.LoadOneAsync<UserProfile>(p => p.UserName == User.Identity.Name)
                          ?? new UserProfile { UserName = User.Identity.Name };
            var routes = HttpRuntime.Cache.Get("config-js-routes") as JsRoute[];
            if(null == routes)throw new InvalidOperationException("The cache for route is empty");

            var modules = routes.AsQueryable()
                .Where(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role) || r.Role == "everybody" || r.Role == "anynomous")
                .Where(r => r.Nav)
                .Where(r => !r.Route.Contains("/:"))
                .Select(r => r.Route)
                .ToList();

            // ReSharper disable RedundantBoolCompare
            var rdlTask = context.LoadAsync(context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user.UserName)), includeTotalRows: true);
            var edTasks = context.LoadAsync(context.EntityDefinitions.Where(e => e.IsPublished == true), includeTotalRows: true);
            var formTask = context.LoadAsync(context.EntityForms.Where(e => e.IsPublished == true), includeTotalRows: true);
            var viewTask = context.LoadAsync(context.EntityViews.Where(e => e.IsPublished == true), includeTotalRows: true);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask, edTasks);


            var reportDefinitionLoadOperation = await rdlTask;
            var entityDefinitionLoadOperation = await edTasks;
            var viewsLoadOperation = await viewTask;
            var formLoadOperation = await formTask;

            var reportDefinitions = new ObjectCollection<ReportDefinition>(reportDefinitionLoadOperation.ItemCollection);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(entityDefinitionLoadOperation.ItemCollection);
            var views = new ObjectCollection<EntityView>(viewsLoadOperation.ItemCollection);
            var forms = new ObjectCollection<EntityForm>(formLoadOperation.ItemCollection);


            while (entityDefinitionLoadOperation.HasNextPage)
            {
                entityDefinitionLoadOperation = await context.LoadAsync(
                        context.EntityDefinitions, entityDefinitionLoadOperation.CurrentPage + 1, includeTotalRows: true);
                entityDefinitions.AddRange(entityDefinitionLoadOperation.ItemCollection);
            }

            while (formLoadOperation.HasNextPage)
            {
                formLoadOperation = await context.LoadAsync(
                        context.EntityForms, formLoadOperation.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(formLoadOperation.ItemCollection);
            }
            while (viewsLoadOperation.HasNextPage)
            {
                viewsLoadOperation = await context.LoadAsync(
                        context.EntityViews, viewsLoadOperation.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewsLoadOperation.ItemCollection);
            }

            while (reportDefinitionLoadOperation.HasNextPage)
            {
                reportDefinitionLoadOperation = await context.LoadAsync(
                        context.ReportDefinitions, reportDefinitionLoadOperation.CurrentPage + 1, includeTotalRows: true);
                reportDefinitions.AddRange(reportDefinitionLoadOperation.ItemCollection);
            }


            modules.AddRange(reportDefinitions.Select(a => string.Format("reportdefinition.execute-id.{0}/{0}", a.Id)));
            modules.AddRange(views.Select(v => v.Route));
            modules.AddRange(entityDefinitions.Select(v => v.Name.ToLowerInvariant()));



            var vm = new UserProfileViewModel
            {
                Profile = profile,
                User = user,
                StartModuleOptions = modules.ToArray(),
                LanguageOptions = new[] { "en", "ms" }
            };

            return View("Script", vm);
        }
    }
}