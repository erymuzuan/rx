using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "HotTowel");
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
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var context = new SphDataContext();
                    var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == model.UserName);
                    if (!profile.HasChangedDefaultPassword)
                        return RedirectToAction("ChangePassword");
                    return RedirectToAction("Index", "HotTowel");
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

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePaswordModel model)
        {
            var userName = User.Identity.Name;

            if (!Membership.ValidateUser(userName, model.OldPassword))
            {
                return Json(new { success = false, status = "PASSWORD_INCORRECT", message = "Your old password is incorrect" });
            }
            if (model.Password != model.ConfirmPassword)
                return Json(new { success = false, status = "PASSWORD_DOESNOT_MATCH", message = "Your password is not the same" });


            var user = Membership.GetUser(userName);
            if (null == user) throw new Exception("Fuck");
            var valid = user.ChangePassword(model.OldPassword, model.Password);
            if (!valid)
                return Json(new { success = false, status = "ERROR_CHANGING_PASSWORD", message = "There's an error changing your password" });

            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == User.Identity.Name);
            profile.HasChangedDefaultPassword = true;

            using (var session = context.OpenSession())
            {
                session.Attach(profile);
                await session.SubmitChanges("Change password");
            }

            if (Request.ContentType == "application/json; charset=utf-8")
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(new { success = true, status = "OK" }));
            }

            return View();
        }

    }

    public class ChangePaswordModel
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
