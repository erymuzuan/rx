using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ComplaintController : Controller
    {
        public async Task<ActionResult> Submit(Complaint complaint)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges("Submit");
            }
            var ticket = string.Format("{0}", complaint.ComplaintId).PadLeft(8, '0');
            complaint.ReferenceNo = ticket;
            
            using (var session = context.OpenSession())
            {
                session.Attach(complaint);
                await session.SubmitChanges("Submit");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(ticket);

        }

    }
}
