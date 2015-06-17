using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static System.IO.File;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class CustomFormController
    {
        [HttpGet]
        [Route("routes")]
        [Authorize(Roles = "developers")]
        public ActionResult GetCustomRoutes()
        {
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            if (Exists(customRouteConfig))
            {
                return File(customRouteConfig, APPLICATION_JSON);
            }
            return Content("[]", APPLICATION_JSON);

        }
        [HttpPost]
        [Route("route")]
        [Authorize(Roles = "developers")]
        public ActionResult SaveCustomRoutes()
        {
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var request = this.GetRequestBody();
            var route = JsonConvert.DeserializeObject<JsRoute>(request);
            if (string.IsNullOrWhiteSpace(route.Route) || route.ModuleId.EndsWith("public.index"))
            {
                this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Json(new { success = false, status = "Not Alllowed", message = "You cannot edit default route" });
            }
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new JsRoute[] { };
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
            }

            var ex = customJsRoutes.SingleOrDefault(x => x.Route == route.Route);
            if (null != ex)
            {
                ex.Caption = route.Caption;
                ex.GroupName = route.GroupName;
                ex.Icon = route.Icon;
                ex.IsAdminPage = route.IsAdminPage;
                ex.Nav = route.Nav;
                ex.Role = route.Role;
                ex.ShowWhenLoggedIn = route.ShowWhenLoggedIn;
                ex.StartPageRoute = route.StartPageRoute;
                ex.Title = route.Title;
            }
            else
            {
                customJsRoutes = customJsRoutes.Concat(new[] { route }).ToArray();
            }
            var text = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, settings);
            WriteAllText(customRouteConfig, text);


            return Content(text, APPLICATION_JSON, Encoding.UTF8);

        }
        [HttpDelete]
        [Route("route/{route}")]
        [Authorize(Roles = "developers")]
        public ActionResult RemoveCustomRoutes(string route)
        {

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new JsRoute[] { };
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
            }

            var ex = customJsRoutes.SingleOrDefault(x => x.Route == route);
            if (null == ex)
                return HttpNotFound("Cannot find custom form with route : " + route);


            var list = customJsRoutes.ToList();
            list.Remove(ex);
            customJsRoutes = list.ToArray();
            var text = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, settings);
            WriteAllText(customRouteConfig, text);


            return Json(new { success = true, status = "OK" });

        }
    }
}