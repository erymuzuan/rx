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
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask);


            var rdls = await rdlTask;
            var routes = new List<JsRoute>();




            var rdlRoutes = from t in rdls.ItemCollection
                            select new JsRoute
                            {
                                Title = t.Title,
                                Route = string.Format("reportdefinition.execute-id.{0}/:id", t.ReportDefinitionId),
                                Caption = t.Title,
                                Icon = "icon-bar-chart",
                                ModuleId = string.Format("viewmodels/reportdefinition.execute-id.{0}", t.ReportDefinitionId)
                            };

            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}