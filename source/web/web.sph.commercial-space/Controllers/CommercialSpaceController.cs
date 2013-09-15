using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class CommercialSpaceController : Controller
    {
        public async Task<ActionResult> SaveCommercialSpace(Space space)
        {
            var buildingId = space.BuildingId;
            var context = new SphDataContext();

            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            space.State = building.Address.State;
            space.City = building.Address.City;
            space.BuildingName = building.Name;
            space.BuildingLot = building.LotNo;

            using (var session = context.OpenSession())
            {
                session.Attach(space);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
