using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class TriggerSetupController : BaseAppController
    {

        public ActionResult Html()
        {
            return View();
        }

        public ActionResult TriggerPathPickerJson(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.Sph.Domain.{0}, domain.sph", id));

            var text = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(text, "", type);
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            return Content("[" + string.Join(",", text) + "]");
        }


        public ActionResult TriggerPathPickerHtml(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.Sph.Domain.{0}, domain.sph", id));

            var text = new StringBuilder();
            TypeHelper.BuildTreeView(text, "", type);
            return Content(text.ToString());
        }



    }
}