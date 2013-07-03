using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ComplaintController : Controller
    {
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
        }

    }
}
