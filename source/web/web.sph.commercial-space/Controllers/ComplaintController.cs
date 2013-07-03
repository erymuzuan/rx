<<<<<<< HEAD
﻿using System;
using System.Threading.Tasks;
=======
﻿using System.Threading.Tasks;
>>>>>>> 02593e57e6d43741e576b5bb9217aa56b966a83a
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ComplaintController : Controller
    {
<<<<<<< HEAD
        public async Task<ActionResult> Submit(Complaint complaint)
        {
            var context = new SphDataContext();
            complaint.Status = "New";
            using (var session = context.OpenSession())
            {
                var ticket = string.Format("{0:yyyy}{1}", DateTime.Today, complaint.ComplaintId).PadLeft(8, '0');
                complaint.ReferenceNo = ticket;

                session.Attach(complaint);
                await session.SubmitChanges("Submit");
            }
            
            
            //this.Response.ContentType = "application/json; charset=utf-8";
            //return Content(ticket);
            return Json(new {status = "success", referenceNo = complaint.ReferenceNo});


=======
        public async Task<ActionResult> Assign(Complaint comp)
        {
            var context = new SphDataContext();
            var complaint = await context.LoadOneAsync<Complaint>(c => c.ComplaintId == comp.ComplaintId);
            complaint.Inspection = comp.Inspection;
            complaint.Status = "In Progress";
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges();
            }
            return Json(true);
>>>>>>> 02593e57e6d43741e576b5bb9217aa56b966a83a
        }

    }
}
<<<<<<< HEAD
                
=======
>>>>>>> 02593e57e6d43741e576b5bb9217aa56b966a83a
