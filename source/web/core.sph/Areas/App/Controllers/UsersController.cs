using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public partial class UsersController : BaseAppController
    {
        public ActionResult Js()
        {
            return View();
        }

        public ActionResult Html()
        {
            return View();
        }
    }
}