using System.Web.Mvc;
using System.Web.Routing;

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
    }
}