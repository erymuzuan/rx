<<<<<<< HEAD
﻿using System.Web.Mvc;
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
=======
﻿using System.Web.Mvc;
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
>>>>>>> 7d25030947e14ce64ad0e9692662c7745ee785e9
}