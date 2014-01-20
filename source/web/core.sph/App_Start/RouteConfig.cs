using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.App_Start
{
    public class RouteConfig
    {
        public async static Task RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            var context = new SphDataContext();
            var query = context.EntityForms;
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var forms = new ObjectCollection<EntityForm>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(lo.ItemCollection);
            }
            foreach (var form in forms)
            {
                routes.MapRoute(
                    name: form.Route,
                    url: string.Format("App/viewmodels/{0}.js", form.Route),
                    defaults: new { controller = "EntityForm", action = "Form", Area = "Sph", id = form.Route }
                );

            }
        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
            // ReSharper disable RedundantBoolCompare
            var rdlTask = context.LoadAsync(context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user)));
            var edTasks = context.LoadAsync(context.EntityDefinitions);
            var formTask = context.LoadAsync(context.EntityForms);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask, edTasks);


            var rdls = await rdlTask;
            var eds = await edTasks; ;
            var forms = await formTask; ;
            var routes = new List<JsRoute>();


            var formRoutes = from t in forms.ItemCollection
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = string.Format("{0}", t.Route.ToLowerInvariant()),
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = string.Format("viewmodels/{0}", t.Route.ToLowerInvariant()),
                                 Nav = false
                             };
            var edRoutes = from t in eds.ItemCollection
                           select new JsRoute
                           {
                               Title = t.Plural,
                               Route = string.Format("{0}", t.Name.ToLowerInvariant()),
                               Caption = t.Plural,
                               Icon = t.IconClass,
                               ModuleId = string.Format("viewmodels/{0}", t.Name.ToLowerInvariant()),
                               Nav = true
                           };

            var rdlRoutes = from t in rdls.ItemCollection
                            select new JsRoute
                            {
                                Title = t.Title,
                                Route = string.Format("reportdefinition.execute-id.{0}/:id", t.ReportDefinitionId),
                                Caption = t.Title,
                                Icon = "icon-bar-chart",
                                ModuleId = string.Format("viewmodels/reportdefinition.execute-id.{0}", t.ReportDefinitionId)
                            };

            routes.AddRange(formRoutes);
            routes.AddRange(edRoutes);
            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}