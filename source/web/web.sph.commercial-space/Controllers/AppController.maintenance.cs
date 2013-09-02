using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
       public ActionResult TemplateMaintenanceHtml()
        {
            return RedirectToAction("Maintenance", "Template");

        }

       public async Task<ActionResult> MaintenanceDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<MaintenanceTemplate>(t => t.MaintenanceTemplateId == templateId);

            return View(template);
        }

    }
}
