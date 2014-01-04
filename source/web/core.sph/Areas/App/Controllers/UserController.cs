using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public partial class UserController : BaseAppController
    {
        public ActionResult UsersJs()
        {
            return View();
        }

        public ActionResult UsersHtml()
        {
            return View();
        }
    }
}