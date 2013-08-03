using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ImageController : Controller
    {

        public async Task<ActionResult> Store(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            if (null == content)
                return Redirect("/images/no-image.png");

            return File(content.Content, MimeMapping.GetMimeMapping(content.FileName));


        }
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
            if (type == typeof (FieldChangeField))
                return RedirectPermanent("~/images/FieldChangeField.png");

            return Content("www");
        }

    }
}
