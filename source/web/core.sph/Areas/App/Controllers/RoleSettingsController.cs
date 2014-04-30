using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Areas.Sph.Controllers;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Web.ViewModels;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class RoleSettingsController : BaseController
    {
        public async Task<ActionResult> Html()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);
            var routeJson = System.IO.File.ReadAllText(routeConfig);
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
                               Route = string.Format("{0}", t.Name.ToLowerInvariant()),
                               Caption = t.Plural,
                               Icon = t.IconClass,
                               ModuleId = string.Format("viewmodels/{0}", t.Name.ToLowerInvariant()),
                               Nav = t.IsShowOnNavigationBar
                           };

            var vm = new RoleSettingViewModel();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(routeJson, settings);


            vm.Roles.ClearAndAddRange(roles);
            vm.Routes.ClearAndAddRange(routes);
            vm.Routes.AddRange(edRoutes);
            return View(vm);
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