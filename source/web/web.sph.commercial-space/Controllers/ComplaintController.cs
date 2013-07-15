﻿using System;
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
            complaint.Status = "New";
 
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges("tenant submit complaint");
            }

            using (var session = context.OpenSession())
            {
                var ticket = string.Format("AD{0:yyyy}{1}", DateTime.Today, complaint.ComplaintId).PadLeft(8, '0');
                complaint.ReferenceNo = ticket;
                session.Attach(complaint);
                await session.SubmitChanges("generate complaint number");
            }

            return Json(new { status = "success", referenceNo = complaint.ReferenceNo });
        }

        public async Task<ActionResult> Assign(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Status = "InProgress";
            complaint.Department = comp.Department;

            var maintenance = new Maintenance
                {
                    Status = "New",
                    Resolution = "Not Started",
                    Department = comp.Department,
                    ComplaintId = comp.ComplaintId,
                    StartDate = null,
                    EndDate = null,
                    Complaint = comp
                };

            using (var session = context.OpenSession())
            {
                session.Attach(complaint,maintenance);
                await session.SubmitChanges();
            }
            return Json(true);
        }
    }
}
