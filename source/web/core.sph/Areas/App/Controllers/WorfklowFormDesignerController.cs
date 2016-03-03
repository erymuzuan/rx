using System.Web.Mvc;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class WorkflowFormDesignerController : BaseAppController
    {
        public ActionResult Index()
        {
            return Content("test-index");
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