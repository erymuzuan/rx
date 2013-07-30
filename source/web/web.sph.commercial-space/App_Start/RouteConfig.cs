using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.App_Start
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
                defaults: new { controller = "HotTowel", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var context = new SphDataContext();
            var buildingTemplates = await context.LoadAsync(context.BuildingTemplates.Where(t => t.IsActive == true));

            var routes = from t in buildingTemplates.ItemCollection
                select new JsRoute
                {
                    Name = t.Name,
                    Url = string.Format("building.detail-templateid.{0}/:templateId/:id", t.BuildingTemplateId),
                    Role = "can_add_commercial_space",
                    Caption = t.Name,
                    Icon = "icon-building",
                    ModuleId = string.Format("viewmodels/building.detail-templateid.{0}", t.BuildingTemplateId),
                };

            return routes;
        } 
    }
}