using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.App_Start;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public async Task<ActionResult> ConfigJs()
        {
            var username = User.Identity.Name;
            var context = new SphDataContext();
            var profileTask =  context.LoadOneAsync<UserProfile>(u => u.Username == username);
            var statesOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "State", x => x.Value);
            var departmentOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Departments", x => x.Value);
            var spaceUsageOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Categories", x => x.Value);

            await Task.WhenAll(profileTask, statesOptionTask, departmentOptionTask, spaceUsageOptionTask);
            var profile = await profileTask;
            var spaceUsageoption = await spaceUsageOptionTask;
            var departmentOptions = await departmentOptionTask;
            var stateOptions = await statesOptionTask;

            var vm = new ApplicationConfigurationViewModel
            {
                StartModule = "public.index",
                StateOptions = string.IsNullOrWhiteSpace(stateOptions) ? "[]" : stateOptions,
                SpaceUsageOptions = string.IsNullOrWhiteSpace(spaceUsageoption) ? "[]" : spaceUsageoption,
                DepartmentOptions =string.IsNullOrWhiteSpace(departmentOptions) ? "[]": departmentOptions,
                UserProfile = profile
            };
            if (null != profile)
            {
                 vm.StartModule = profile.StartModule;
            }
           
            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);
            
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings).AsQueryable()
                .WhereIf(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role), User.Identity.IsAuthenticated)
                .WhereIf(r => string.IsNullOrWhiteSpace(r.Role), !User.Identity.IsAuthenticated);
            vm.Routes.AddRange(routes);

            vm.Routes.AddRange(await RouteConfig.GetJsRoutes());

            return View(vm);
        }

        [ActionName("config.js")]
        public ActionResult Config(string id)
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            var script = this.RenderScript("ConfigJs");
            return Content(script);
        }


    }
}
