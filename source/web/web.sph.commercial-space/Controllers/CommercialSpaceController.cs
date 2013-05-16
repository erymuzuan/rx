using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class CommercialSpaceController : Controller
    {
        public async Task<ActionResult> SaveCommercialSpace(CommercialSpace commercialSpace)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(commercialSpace);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
