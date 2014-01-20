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
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
            // ReSharper disable RedundantBoolCompare
            var rdlTask = context.LoadAsync(context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user)));
            var edTasks = context.LoadAsync(context.EntityDefinitions);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask,edTasks);


            var rdls = await rdlTask;
            var eds = await edTasks;
            var routes = new List<JsRoute>();


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

            routes.AddRange(edRoutes);
            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}