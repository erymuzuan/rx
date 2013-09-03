using System.Web.Mvc;
using System.Web.Security;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","HotTowel");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogin(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    return Json(new { success = true, redirect = returnUrl });
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return Json(true);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("Index" ,"HotTowel");
                }
                var user = Membership.GetUser(model.UserName);
                if (null != user && user.IsLockedOut)
                    ModelState.AddModelError("", "Your acount has beeen locked, Please contact NSRM administrator.");
                else
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }



        public ActionResult Flyout()
        {
            return View();
        }


        public ActionResult ChangePassword()
        {
            return View();
        }

    }

    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
