using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult RoleSettingsJs()
        {
            var roles = new[] { Roles.ADMIN_DASHBOARD , Roles.ADMIN_SETTING};
            return View(roles);
        }
    }
}