using System.Web.Mvc;

namespace web.sph.portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

  
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

    }
}
