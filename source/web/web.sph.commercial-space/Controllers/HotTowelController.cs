using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    [Authorize]
    public class HotTowelController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
