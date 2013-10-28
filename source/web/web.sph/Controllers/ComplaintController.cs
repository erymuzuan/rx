using System;
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class ComplaintController : Controller
    {

        public async Task<ActionResult> Submit(Complaint complaint)
        {
            var context = new SphDataContext();
            complaint.Status = "Baru";

            var template = await context.LoadOneAsync<ComplaintTemplate>(s => s.ComplaintTemplateId == complaint.TemplateId);
            var result = complaint.ValidateBusinessRule(template.BusinessRuleCollection);
            if (result.Success == false)
            {
                return Json(result);
            }

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
            result.ReferenceNo = complaint.ReferenceNo;
            return Json(result);
        }

        public async Task<ActionResult> Assign(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Status = "Dalam Proses";
            complaint.Department = comp.Department;

            var maintenance = new Maintenance
                {
                    Status = "Baru",
                    Resolution = "Belum Dilaksanakan",
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

        public async Task<ActionResult> Close(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Status = "Ditutup";
           
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges();
            }
            return Json(true);
        }
    }
}
