using System.Web.Mvc;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class PartialViewDesignerController : BaseAppController
    {
        public ActionResult Html()
        {
            var vm = new TemplateFormViewModel { IsImportVisible = true };
            return View(vm);
        }

    }
}