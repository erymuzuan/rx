using System;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                var ticket = string.Format("{0:yyyy}{1}", DateTime.Today, complaint.ComplaintId).PadLeft(8, '0');
                complaint.ReferenceNo = ticket;

                session.Attach(complaint);
                await session.SubmitChanges("Submit");
            }
            
            
            //this.Response.ContentType = "application/json; charset=utf-8";
            //return Content(ticket);
            return Json(new {status = "success", referenceNo = complaint.ReferenceNo});


        }

    }
}
                