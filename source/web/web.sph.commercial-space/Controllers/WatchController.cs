using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class WatchController : Controller
    {
        public async Task<ActionResult> Register(string entity, int id)
        {
            await Task.Delay(2500);
            return Json(true);
        }

        public async Task<ActionResult> Deregister(string entity, int id)
        {
            await Task.Delay(2500);
            return Json(true);
        }

        public async Task<ActionResult> GetWatch(string entity, int id)
        {
            await Task.Delay(2500);
            return Json(id % 2 == 0, JsonRequestBehavior.AllowGet);
        }

    }
}
