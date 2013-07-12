using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        //
        // GET: /AppController.tenantportal/

        public ActionResult TenantPortalHtml()
        {
            return View();
        }  
        public ActionResult TenantPortalJs()
        {
            return View();
        }

    }
}
