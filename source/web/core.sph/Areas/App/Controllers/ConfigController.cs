using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class ConfigController : BaseAppController
    {
        [RazorScriptFilter]
        [NoCache]
        public async Task<ActionResult> Js()
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            var userName = User.Identity.Name;
            var context = new SphDataContext();
            var profileTask = context.LoadOneAsync<UserProfile>(u => u.UserName == userName);
            var statesOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "State", x => x.Value);
            var departmentOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Departments", x => x.Value);
            var spaceUsageOptionTask = context.GetScalarAsync<Setting, string>(x => x.Key == "Categories", x => x.Value);

            await Task.WhenAll(profileTask, statesOptionTask, departmentOptionTask, spaceUsageOptionTask);
            var profile = await profileTask;
            var departmentOptions = await departmentOptionTask;
            var stateOptions = await statesOptionTask;

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

            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings).AsQueryable()
                .WhereIf(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role) || r.Role == "everybody", User.Identity.IsAuthenticated)
                .WhereIf(r => string.IsNullOrWhiteSpace(r.Role), !User.Identity.IsAuthenticated);
            vm.Routes.AddRange(routes);
            
            return Script("Index", vm);
        }


    }
}