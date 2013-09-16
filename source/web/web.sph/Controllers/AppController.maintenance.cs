using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
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

       public async Task<ActionResult> MaintenanceAssignmentHtml(int id)
       {
           var context = new SphDataContext();
           var maintenance = await context.LoadOneAsync<Maintenance>(m => m.MaintenanceId == id);
           var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == maintenance.Complaint.TemplateId);

           return View(template);
       }
    }
}
