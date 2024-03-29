using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class HeaderController : BaseAppController
    {
        [NoCache]
        public async Task<ActionResult> Html()
        {
            if (!User.Identity.IsAuthenticated)
                return View(new SphIndexViewModel {Designation = new Designation {IsHelpVisible = false}});
            
            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(ua => ua.UserName == User.Identity.Name);
            if (null == profile)
                return View(new SphIndexViewModel { Designation = new Designation { IsHelpVisible = false } });

            ViewBag.StartModule = "#" + profile.StartModule;

            var designation = (await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation)) ?? new Designation { IsHelpVisible = true, HelpUri = "/docs/" };
            designation.HelpUri = string.IsNullOrWhiteSpace(designation.HelpUri) ? "/docs/" : designation.HelpUri;
            var vm = new SphIndexViewModel
            {
                Profile = profile,
                Designation = designation,
                LoginUrl = System.Web.Security.FormsAuthentication.LoginUrl
            };
            return View(vm);
        }
    }
}