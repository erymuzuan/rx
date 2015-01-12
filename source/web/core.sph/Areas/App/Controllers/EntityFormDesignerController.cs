using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class EntityFormDesignerController : BaseAppController
    {
        public ActionResult Html()
        {
            var vm = new TemplateFormViewModel
            {
                IsImportVisible = true
            };
            return View(vm);
        }

        public ActionResult LayoutOptions()
        {
            var folder = Server.MapPath("~/Views/EntityFormRenderer");
            var layouts = System.IO.Directory.GetFiles(folder, "*.cshtml")
                .Select(System.IO.Path.GetFileNameWithoutExtension)
                .ToList();

            layouts.Insert(0, "Html2ColsCustomLeft");
            layouts.Insert(0, "Html2ColsWithAuditTrail");


            return Json(layouts.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}