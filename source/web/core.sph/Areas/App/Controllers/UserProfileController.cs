using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UserProfileController : BaseAppController
    {
        public ActionResult Script()
        {
            return View();
        }
        public ActionResult Html()
        {
            return View();
        }

        public async Task<ActionResult> Js()
        {
            var context = new SphDataContext();
            var user = Membership.GetUser();
            var profile = await context.LoadOneAsync<UserProfile>(p => p.Username == User.Identity.Name)
                          ?? new UserProfile { Username = User.Identity.Name };

            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var modules = JsonConvert.DeserializeObject<JsRoute[]>(json, settings).AsQueryable()
                .Where(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role))
                .Where(r => r.Nav)
                .Select(r => r.Route)
                .ToArray();

            var vm = new UserProfileViewModel
            {
                Profile = profile,
                User = user,
                StartModuleOptions = modules
            };

            var script = this.RenderRazorViewToJs("Script", vm);
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            return Content(script);
        }
    }
}