using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class ActivityScreenController : BaseAppController
    {
        [RazorScriptFilter]
        public ActionResult Js()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            return View("Script", vm);
        }

    

        [OutputCache(Duration = 600)]
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