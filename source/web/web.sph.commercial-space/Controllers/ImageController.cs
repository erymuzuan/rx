using System;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ImageController : Controller
    {

        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/blank.png");
            var type = Type.GetType(id);
            if (type == typeof (DocumentField))
                return RedirectPermanent("~/images/documentfield.png");
            if (type == typeof (FunctionField))
                return RedirectPermanent("~/images/FunctionField.png");
            if (type == typeof (ConstantField))
                return RedirectPermanent("~/images/ConstantField.png");
            if (type == typeof (SetterAction))
                return RedirectPermanent("~/images/SetterAction.png");
            if (type == typeof (EmailAction))
                return RedirectPermanent("~/images/EmailAction.png");

            return Content("www");
        }

    }
}
