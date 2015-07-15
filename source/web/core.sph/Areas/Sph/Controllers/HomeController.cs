using System.Web.Mvc;
using System.Web.UI;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "developers,admin")]
    public class HomeController : Controller
    {
        [OutputCache(Duration = 604800, Location = OutputCacheLocation.Any)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
