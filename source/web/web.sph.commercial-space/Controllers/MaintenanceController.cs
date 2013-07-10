using System;
using System.Collections.Generic;
using System.Linq;
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
            //var maint = await context.LoadOneAsync<Maintenance>(m => m.MaintenanceId == maintenance.MaintenanceId);
            //maint.Status = "Inspection";
            //var workOrderNo = string.Format("WO{0:yyyy}{1}", DateTime.Today, maintenance.MaintenanceId).PadLeft(8, '0');
            //maint.WorkOrderNo = workOrderNo;

            using (var session = context.OpenSession())
            {
                session.Attach(maintenance);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
