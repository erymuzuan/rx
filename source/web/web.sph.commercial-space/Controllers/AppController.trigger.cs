using System;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public ActionResult TriggerPathPickerHtml(string id)
        {
            var type = Type.GetType(string.Format("Bespoke.SphCommercialSpaces.Domain.{0}, domain.commercialspace, Version=1.0.2.1006, Culture=neutral", id));
            var vm = new TriggerPathPickerHtmlViewModel
                {
                    Type = type,
                    Path = ""
                };
            return View("TriggerPathPickerHtml", vm);
        }

        public ActionResult TriggerPathPickerJs()
        {
            return View();
        }

    }

    public class TriggerPathPickerHtmlViewModel
    {
        public Type Type { get; set; }
        public string Path { get; set; }
    }
}
