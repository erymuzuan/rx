using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ComplaintTemplateController : Controller
    {
        public async Task<ActionResult> Save(ComplaintTemplate complainttemplate)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(complainttemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
