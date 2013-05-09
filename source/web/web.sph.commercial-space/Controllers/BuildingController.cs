using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.CommercialSpace.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class BuildingController : Controller
    {
       
        public async Task<ActionResult> SaveBuilding(Building building)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(building);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
