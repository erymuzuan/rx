using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public ActionResult TriggerSetupHtml()
        {
            return View();
        }

        public ActionResult TriggerPathPickerJson(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace", id));

            var text = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(text, "", type);
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            return Content("[" + string.Join(",", text) + "]");
        }


        public ActionResult TriggerPathPickerHtml(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace", id));

            var text = new StringBuilder();
            TypeHelper.BuildTreeView(text, "", type);
            return Content(text.ToString());
        }



    }
}
