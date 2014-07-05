using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class EntityFormDesignerController : BaseAppController
    {
        [RazorScriptFilter]
        //[Route("~/sphapp/viewmodels/entity.form.designer.js")]
        public ActionResult Js()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            return View("Script", vm);
        }

        public ActionResult Script()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            return View(vm);
        }
       // [Route("~/sphapp/view/entity.form.designer.html")]
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