using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
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
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);
            var context = new SphDataContext();

            var edQuery = context.EntityDefinitions.Where(e => e.IsPublished == true);
            var edLo = await context.LoadAsync(edQuery, includeTotalRows: true);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(edLo.ItemCollection);


            while (edLo.HasNextPage)
            {
                edLo = await context.LoadAsync(edQuery, edLo.CurrentPage + 1, includeTotalRows: true);
                entityDefinitions.AddRange(edLo.ItemCollection);
            }
            var edRoutes = from t in entityDefinitions
                           select new JsRoute
                           {
                               Title = t.Plural,
                               Route = $"{t.Name.ToLowerInvariant()}",
                               Caption = t.Plural,
                               Icon = t.IconClass,
                               ModuleId = $"viewmodels/{t.Name.ToLowerInvariant()}",
                               Nav = t.IsShowOnNavigationBar
                           };

            var vm = new RoleSettingViewModel();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);
            var routes = this.GetJsRoutes();


            vm.Roles.ClearAndAddRange(roles);
            vm.Routes.ClearAndAddRange(routes);
            vm.Routes.AddRange(edRoutes);
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
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);

            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderRazorViewToJs("Script", roles);
            return Content(script);


        }
        public ActionResult Script()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);

            return View(roles.AsEnumerable());
        }
    }
}