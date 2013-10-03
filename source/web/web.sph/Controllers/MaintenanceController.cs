using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    public class MaintenanceController : Controller
    {

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

        public async Task<ActionResult> Assign()
        {
            var context = new SphDataContext();
            var data = this.GetRequestJson<Maintenance>();
            var maint = await context.LoadOneAsync<Maintenance>(m => m.MaintenanceId ==data.MaintenanceId) ?? new Maintenance();
            maint.Status = "Pemeriksaan";
            maint.Officer = data.Officer;
            maint.TemplateId = data.TemplateId;
            maint.StartDate = data.StartDate;
            maint.EndDate = data.EndDate;

            var workOrderNo = string.Format("WO{0:yyyy}{1}", DateTime.Today, data.MaintenanceId);
            maint.WorkOrderNo = workOrderNo;

            var workorder = new WorkOrder
                {
                    No = maint.WorkOrderNo,
                    MaintenanceId = maint.MaintenanceId,
                    TemplateId = maint.TemplateId,
                    Status = "Baru",
                    Officer = maint.Officer,
                    Department = maint.Department,
                    StartDate = maint.StartDate ?? DateTime.Today,
                    EndDate = maint.EndDate ?? DateTime.Today.AddDays(7),
                    Resolution = "Belum Dilaksanakan"
                };
            using (var session = context.OpenSession())
            {
                session.Attach(maint,workorder);
                await session.SubmitChanges();
            }
            return Json(new { status = "success", officer = maint.Officer });
        }
        
        public async Task<ActionResult> Closed(int id)
        {
            var context = new SphDataContext();
            var maint = await context.LoadOneAsync<Maintenance>(m => m.MaintenanceId == id);
            maint.Status = "Selesai";
          
            using (var session = context.OpenSession())
            {
                session.Attach(maint);
                await session.SubmitChanges();
            }
            return Json(new { status = "success", officer = maint.Officer });
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
