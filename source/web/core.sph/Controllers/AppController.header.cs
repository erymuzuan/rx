using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public async  Task<ActionResult> HeaderHtml()
        {
            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(ua => ua.Username == User.Identity.Name);
            if (null != profile)
                ViewBag.StartModule = profile.StartModule;
           return View();
        }
    }
}