using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class MaintenanceController : Controller
    {
        public async Task<ActionResult> Assign(Maintenance maintenance)
        {
            var context = new SphDataContext();
            var maint = await context.LoadOneAsync<Maintenance>(m => m.MaintenanceId == maintenance.MaintenanceId);
            maint.Status = "Inspection";
            maint.Officer = maintenance.Officer;
            var workOrderNo = string.Format("WO{0:yyyy}{1}", DateTime.Today, maintenance.MaintenanceId);
            maint.WorkOrderNo = workOrderNo;

            using (var session = context.OpenSession())
            {
                session.Attach(maint);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> Save(Maintenance maintenance)
        {
            var context = new SphDataContext();
            
            using (var session = context.OpenSession())
            {
                session.Attach(maintenance);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> GenerateWorkOrder(int id = 0)
        {
            var maintenanceId = id != 0 ? id : 1;
            var context = new SphDataContext();
            var maintenance = await context.LoadOneAsync<Maintenance>(c => c.MaintenanceId == maintenanceId);
            
            var export = ObjectBuilder.GetObject<IWorkOrderExport>();
            var filename = string.Format("{0}-{1:MMyyyy}.workOrder.xlsx", maintenance.MaintenanceId, DateTime.Today);
            var temp = System.IO.Path.GetTempFileName() + ".xlsx";

            export.GenerateWorkOrder(maintenance, temp);

            this.Response.ContentType = "application/json";
            return File(System.IO.File.ReadAllBytes(temp), MimeMapping.GetMimeMapping(".xlsx"), filename);
        }

    }
}
