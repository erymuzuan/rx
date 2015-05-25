using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class HeaderController : BaseAppController
    {
        [NoCache]
        public async Task<ActionResult> Html()
        {
            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(ua => ua.UserName == User.Identity.Name);
            if (null != profile)
                ViewBag.StartModule = "#" + profile.StartModule;
            return View();
        }
    }
}