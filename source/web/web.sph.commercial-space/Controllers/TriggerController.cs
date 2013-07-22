using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TriggerController : Controller
    {
        
        public async Task<ActionResult> Save(Trigger trigger)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Submit trigger");
            }
            return Json(true);
        }

    }
}
