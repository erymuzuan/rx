using System.Web.Mvc;
using System.Web.Security;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult UserProfileHtml()
        {
            return View();
        }
        public ActionResult UserProfileJs()
        {
            var member = Membership.GetUser();
            return View(member);
        }
    }
}