using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.ViewModels;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult ReportDefinitionEditHtml()
        {
            var vm = new ReportBuilderViewModel();
            return View(vm);
        }
    }
}
