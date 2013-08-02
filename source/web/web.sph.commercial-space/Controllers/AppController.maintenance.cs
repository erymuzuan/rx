using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public async Task<ActionResult> MaintenanceFormHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

            return View(template);
        }

        public ActionResult TemplateMaintenanceHtml()
        {
            return RedirectToAction("Maintenance", "Template");

        }

        public async Task<ActionResult> MaintenanceDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

            return View(template);
        }
        
        public ActionResult MaintenanceListHtml()
        {
            return View();
        }

        public ActionResult MaintenanceListJs()
        {
            return View();
        }

    }
}
