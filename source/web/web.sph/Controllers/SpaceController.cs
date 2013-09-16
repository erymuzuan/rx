using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class SpaceController : Controller
    {
        public async Task<ActionResult> Save(Space space)
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
