using System;
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ComplaintController : Controller
    {

        public async Task<ActionResult> Submit(Complaint complaint)
        {
            var context = new SphDataContext();
            complaint.Status = "Baru";
            var inspection = new Inspection{Resolution = "Belum Diperiksa"};
            complaint.Inspection = inspection;

            using (var session = context.OpenSession())
            {
                var ticket = string.Format("AD{0:yyyy}{1}", DateTime.Today, complaint.ComplaintId).PadLeft(8, '0');
                complaint.ReferenceNo = ticket;

                session.Attach(complaint);
                await session.SubmitChanges("Submit");
            }

            return Json(new {status = "success", referenceNo = complaint.ReferenceNo});
        }

        public async Task<ActionResult> Assign(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Inspection = comp.Inspection;
            complaint.Inspection.AssignedDate = DateTime.Today;
            complaint.Status = "Dalam Proses";
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> UpdateInspection(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Inspection.InspectionDate = comp.Inspection.InspectionDate;
            complaint.Inspection.Resolution = comp.Inspection.Resolution;
            complaint.Inspection.Observation = comp.Inspection.Observation;
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
