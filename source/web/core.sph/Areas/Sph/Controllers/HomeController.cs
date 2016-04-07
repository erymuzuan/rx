using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class HomeController : Controller
    {
        [NoCache]
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "SphAccount");

            if (!User.IsInRole("administrators") || !User.IsInRole("developers"))
                return RedirectToAction("Logoff", "SphAccount");

            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(ua => ua.UserName == User.Identity.Name);
            if (null == profile)
                return View("ide.shell", new SphIndexViewModel { Designation = new Designation { IsHelpVisible = false } });

            ViewBag.StartModule = "#" + profile.StartModule;

            var designation = (await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation)) ?? new Designation { IsHelpVisible = true, HelpUri = "/docs/" };
            designation.HelpUri = string.IsNullOrWhiteSpace(designation.HelpUri) ? "/docs/" : designation.HelpUri;
            var vm = new SphIndexViewModel
            {
                Profile = profile,
                Designation = designation
            };

            return View("ide.shell", vm);
        }

    }
   
}
