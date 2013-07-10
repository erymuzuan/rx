<<<<<<< HEAD
﻿using System.Web.Mvc;
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
=======
﻿using System.Web.Mvc;
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
>>>>>>> 7d25030947e14ce64ad0e9692662c7745ee785e9
}