using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        [Authorize(Roles = Roles.ADMIN_USERS)]
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
