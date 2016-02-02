using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UserProfileController : BaseAppController
    {
        public ActionResult Html()
        {
            return View();
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js()
        {
            var context = new SphDataContext();
            var user = Membership.GetUser();
            var profile = await context.LoadOneAsync<UserProfile>(p => p.UserName == User.Identity.Name)
                          ?? new UserProfile { UserName = User.Identity.Name };
            var routes = HttpRuntime.Cache.Get("config-js-routes") as JsRoute[];
            if (null == routes) throw new InvalidOperationException("The cache for route is empty");

            var modules = routes.AsQueryable()
                .Where(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role) || r.Role == "everybody" || r.Role == "anynomous")
                .Where(r => r.Nav)
                .Where(r => !r.Route.Contains("/:"))
                .Select(r => r.Route)
                .ToList();
            

            var reportDefinitions = context.LoadFromSources<ReportDefinition>(t => t.IsActive || (t.IsPrivate && t.CreatedBy == user.UserName));
            var views = context.LoadFromSources<EntityView>(e => e.IsPublished);

            modules.AddRange(reportDefinitions.Select(a => string.Format("reportdefinition.execute-id.{0}/{0}", a.Id)));


            foreach (var vw in views)
            {
                var users = await vw.Performer.GetUsersAsync(vw);
                if(users.Contains(profile.UserName))
                    modules.Add(vw.Name.ToLowerInvariant());
            }



            var vm = new UserProfileViewModel
            {
                Profile = profile,
                User = user,
                StartModuleOptions = modules.OrderBy(m => m).ToArray(),
                LanguageOptions = new[] { "en", "ms" }
            };

            return View("Script", vm);
        }

        [Authorize]
        public async Task<ActionResult> UpdateUser(UserProfile profile)
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadOneAsync<UserProfile>(p => p.UserName == User.Identity.Name)
                ?? new UserProfile();
            userprofile.UserName = User.Identity.Name;
            userprofile.Email = profile.Email;
            userprofile.Telephone = profile.Telephone;
            userprofile.FullName = profile.FullName;
            userprofile.StartModule = profile.StartModule;
            userprofile.Language = profile.Language;

            if (userprofile.IsNewItem) userprofile.Id = userprofile.UserName.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(userprofile);
                await session.SubmitChanges();
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(userprofile));


        }
    }
}