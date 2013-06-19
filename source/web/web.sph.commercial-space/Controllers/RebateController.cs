using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RebateController : Controller
    {
        public async Task<ActionResult> Save(Rebate rebate)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(rebate);
                await session.SubmitChanges();
            }

            return Json(true);
        }

    }
}
