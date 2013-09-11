using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
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