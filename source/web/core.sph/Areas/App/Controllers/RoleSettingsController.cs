using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Areas.Sph.Controllers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class RoleSettingsController : BaseController
    {
        public async Task<ActionResult> Html()
        {
            var context = new SphDataContext();
     

            var vm = new RoleSettingViewModel();
            var roles = Roles.GetAllRoles();
            var routes = this.GetJsRoutes();
            var entities = await context.GetListAsync<EntityDefinition, string>(e => e.Id != "0", e => e.Name);
            vm.SearchableEntityOptions.AddRange(entities);

            vm.Roles.ClearAndAddRange(roles);
            vm.Routes.ClearAndAddRange(routes);
            return View(vm);
        }

        private JsRoute[] GetJsRoutes()
        {
            JsRoute[] configJsRoutes;
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

            var customRouteConfig = Server.MapPath(ConfigController.CustomRouteConfig);
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
            return configJsRoutes;
        }
        public ActionResult Js()
        {
            var roles = Roles.GetAllRoles();

            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderRazorViewToJs("Script", roles);
            return Content(script);


        }
        public ActionResult Script()
        {
            var roles = Roles.GetAllRoles();
            return View(roles.AsEnumerable());
        }
    }
}