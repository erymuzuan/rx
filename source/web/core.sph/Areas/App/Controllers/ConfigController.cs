using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    [RoutePrefix("config")]
    public class ConfigController : BaseAppController
    {
        [RazorScriptFilter]
        [NoCache]
        public async Task<ActionResult> Js()
        {
            var userName = User.Identity.Name;
            var context = new SphDataContext();
            var profileTask = context.LoadOneAsync<UserProfile>(u => u.UserName == userName);
            var statesOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "State", x => x.Value);
            var departmentOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Departments", x => x.Value);
            var spaceUsageOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Categories", x => x.Value);
            var jsRoutesTask = RouteConfig.GetJsRoutes();

            await Task.WhenAll(profileTask, statesOptionTask, departmentOptionTask, spaceUsageOptionTask, jsRoutesTask);
            var profile = await profileTask;
            var departmentOptions = await departmentOptionTask;
            var stateOptions = await statesOptionTask;
            var jsRoutes = await jsRoutesTask;

            var vm = new ApplicationConfigurationViewModel
            {
                StartModule = "public.index",
                StateOptions = string.IsNullOrWhiteSpace(stateOptions) ? "[]" : stateOptions,
                DepartmentOptions = string.IsNullOrWhiteSpace(departmentOptions) ? "[]" : departmentOptions,
                UserProfile = profile
            };
            if (null != profile)
            {
                vm.StartModule = profile.StartModule;
                vm.Routes.Add(new JsRoute
                {
                    GroupName = "default",
                    Route = "",
                    ModuleId = "viewmodels/" + profile.StartModule,
                    ShowWhenLoggedIn = true

                });
            }


            var configJsRoutes = HttpRuntime.Cache.Get("config-js-routes") as JsRoute[];
            if (null == configJsRoutes)
            {
                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var assembly = Assembly.GetExecutingAssembly();
                const string RESOURCE_NAME = "Bespoke.Sph.Web.routes.config.json";
                JsRoute[] systemRoutes;
                using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
                {
                    if (null == stream) throw new InvalidOperationException("Missing routes.config.json resource in core.sph.dll");
                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        systemRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                    }
                }

                var customRouteConfig = Server.MapPath(CustomRouteConfig);
                if (System.IO.File.Exists(customRouteConfig))
                {
                    var json = System.IO.File.ReadAllText(customRouteConfig);
                    var customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                    configJsRoutes = customJsRoutes.Concat(systemRoutes).ToArray();
                }
                else
                {
                    configJsRoutes = systemRoutes;
                }
                HttpRuntime.Cache.Insert("config-js-routes", configJsRoutes, new CacheDependency(customRouteConfig));
            }

            var routes = configJsRoutes.AsQueryable()
                .WhereIf(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role) || r.Role == "everybody", User.Identity.IsAuthenticated).WhereIf(r => string.IsNullOrWhiteSpace(r.Role), !User.Identity.IsAuthenticated);
            vm.Routes.AddRange(routes);

            vm.Routes.AddRange(jsRoutes);

            return Script("Index", vm);
        }
        const string CustomRouteConfig = "~/App_Data/routes.config.json";

        [HttpGet]
        [Route("custom-routes")]
        [Authorize(Roles = "developers")]
        public ActionResult GetCustomRoutes()
        {
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            if (System.IO.File.Exists(customRouteConfig))
            {
                return File(customRouteConfig, APPLICATION_JSON);
            }
            return Content("[]", APPLICATION_JSON);

        }
        [HttpPost]
        [Route("custom-route")]
        [Authorize(Roles = "developers")]
        public ActionResult SaveCustomRoutes()
        {

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var request = this.GetRequestBody();
            var route = JsonConvert.DeserializeObject<JsRoute>(request);
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new JsRoute[] { };
            if (System.IO.File.Exists(customRouteConfig))
            {
                var json = System.IO.File.ReadAllText(customRouteConfig);
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
            System.IO.File.WriteAllText(customRouteConfig, text);


            return Content(text, APPLICATION_JSON, Encoding.UTF8);

        }
        [HttpDelete]
        [Route("custom-route/{route}")]
        [Authorize(Roles = "developers")]
        public ActionResult RemoveCustomRoutes(string route)
        {

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new JsRoute[] { };
            if (System.IO.File.Exists(customRouteConfig))
            {
                var json = System.IO.File.ReadAllText(customRouteConfig);
                customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
            }

            var ex = customJsRoutes.SingleOrDefault(x => x.Route == route);
            if (null == ex)
                return HttpNotFound("Cannot find custom form with route : " + route);
            

            var list = customJsRoutes.ToList();
            list.Remove(ex);
            customJsRoutes = list.ToArray();
            var text = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, settings);
            System.IO.File.WriteAllText(customRouteConfig, text);


            return Json(new { success = true, status = "OK" });

        }
    }
}