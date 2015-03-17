using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{

    [Authorize]
    public class SphAccountController : Controller
    {
        public async Task<ActionResult> Logoff()
        {
            FormsAuthentication.SignOut();
            try
            {
                var logger = ObjectBuilder.GetObject<ILogger>();
                await logger.LogAsync(new LogEntry { Log = EventLog.Security, Message = "Logoff" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var setting = new Setting { UserName = email, Key = "ForgotPassword", Value = DateTime.Now.ToString("s"), Id = Strings.GenerateId() };
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(setting);
                await session.SubmitChanges("ForgotPassword");
            }
            using (var smtp = new SmtpClient())
            {
                var mail = new MailMessage(ConfigurationManager.FromEmailAddress, email)
                {
                    Subject = ConfigurationManager.ApplicationFullName + " Forgot password ",
                    Body = string.Format("{0}/Sph/SphAccount/ResetPassword/{1}", ConfigurationManager.BaseUrl, setting.Id)
                };
                await smtp.SendMailAsync(mail);
            }
            return Json(new { sucess = true, status = "ok" });
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
            var logger = ObjectBuilder.GetObject<ILogger>();
            if (ModelState.IsValid)
            {
                var directory = ObjectBuilder.GetObject<IDirectoryService>();
                if (await directory.AuthenticateAsync(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var context = new SphDataContext();
                    var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == model.UserName);
                    await logger.LogAsync(new LogEntry { Log = EventLog.Security });
                    if (null != profile)
                    {
                        if (!profile.HasChangedDefaultPassword)
                            return RedirectToAction("ChangePassword");
                        if (returnUrl == "/" ||
                            returnUrl.Equals("/sph", StringComparison.InvariantCultureIgnoreCase) ||
                            returnUrl.Equals("/sph#", StringComparison.InvariantCultureIgnoreCase) ||
                            returnUrl.Equals("/sph/", StringComparison.InvariantCultureIgnoreCase) ||
                            returnUrl.Equals("/sph/#", StringComparison.InvariantCultureIgnoreCase) ||
                            string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect("/sph#" + profile.StartModule);
                    }
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home", new { area = "Sph" });
                }
                var user = await directory.GetUserAsync(model.UserName);
                await logger.LogAsync(new LogEntry { Log = EventLog.Security, Message = "Login Failed" });
                if (null != user && user.IsLockedOut)
                    ModelState.AddModelError("", "Your acount has beeen locked, Please contact your administrator.");
                else
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }



            return View(model);
        }



        public ActionResult Flyout()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var context = new SphDataContext();
            var setting = await context.LoadOneAsync<Setting>(x => x.Id == id);
            var model = new ResetPaswordModel { IsValid = true };

            if (null == setting)
            {
                model.IsValid = false;
                model.Mesage = "The link is invalid";
                return View(model);
            }

            model.Email = setting.UserName;
            if ((DateTime.Now - setting.CreatedDate).TotalMinutes > 10)
            {
                model.IsValid = false;
                model.Mesage = "The link has expired";
                return View(model);

            }
            var user = Membership.FindUsersByEmail(setting.UserName);
            if (user.Count == 0)
            {
                model.IsValid = false;
                model.Mesage = "Cannot find any user with email  " + model.Email;
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPaswordModel model)
        {
            var username = Membership.GetUserNameByEmail(model.Email);
            if (model.Password != model.ConfirmPassword)
                return Json(new { success = false, status = "PASSWORD_DOESNOT_MATCH", message = "Your password is not the same" });

            var user = Membership.GetUser(username);
            if (null == user) throw new Exception("Fuck");
            var temp = user.ResetPassword();
            user.ChangePassword(temp, model.Password);

            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == username)
                ?? new UserProfile
                {
                    UserName = username,
                    Email = model.Email,
                    HasChangedDefaultPassword = true,
                    Id = username,
                    StartModule = "public.index"
                };
            profile.HasChangedDefaultPassword = true;

            using (var session = context.OpenSession())
            {
                session.Attach(profile);
                await session.SubmitChanges("Change password");
            }

            if (Request.ContentType.Contains("application/json"))
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(new { success = true, status = "OK" }));
            }

            return Redirect("/");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword(ChangePaswordModel model)
        {
            var userName = User.Identity.Name;

            if (!Membership.ValidateUser(userName, model.OldPassword))
            {
                return Json(new { success = false, status = "PASSWORD_INCORRECT", message = "Your old password is incorrect", user = userName });
            }
            if (model.Password != model.ConfirmPassword)
                return Json(new { success = false, status = "PASSWORD_DOESNOT_MATCH", message = "Your password is not the same" });


            var user = Membership.GetUser(userName);
            if (null == user) throw new Exception("Fuck");
            var valid = user.ChangePassword(model.OldPassword, model.Password);
            if (!valid)
                return Json(new { success = false, status = "ERROR_CHANGING_PASSWORD", message = "There's an error changing your password" });

            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(u => u.UserName == User.Identity.Name);
            profile.HasChangedDefaultPassword = true;

            using (var session = context.OpenSession())
            {
                session.Attach(profile);
                await session.SubmitChanges("Change password");
            }

            if (Request.ContentType.Contains("application/json"))
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(new { success = true, status = "OK" }));
            }

            return Redirect("/");
        }

    }

    public class ResetPaswordModel
    {
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsValid { get; set; }
        public string Mesage { get; set; }
    }

    public class ChangePaswordModel
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }
    }

    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
