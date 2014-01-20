using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class EntityFormDesignerController : BaseAppController
    {
        public ActionResult Js()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderRazorViewToJs("Script", vm);
            return Content(script);


        }

        public ActionResult Script()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            return View(vm);
        }
        public ActionResult Html()
        {
            var vm = new TemplateFormViewModel
            {
                IsImportVisible = true
            };
            return View(vm);
        }
    }
}