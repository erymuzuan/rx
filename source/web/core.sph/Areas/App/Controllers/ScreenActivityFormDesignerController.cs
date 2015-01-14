using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class ScreenActivityFormDesignerController : BaseAppController
    {
        public ActionResult Html()
        {
            var vm = new TemplateFormViewModel
            {
                IsImportVisible = true
            };
            ObjectBuilder.ComposeMefCatalog(vm);
            return View(vm);
        }
    }
}