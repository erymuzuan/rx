using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class CommercialSpaceController : Controller
    {
        public async Task<ActionResult> SaveCommercialSpace(CommercialSpace commercialSpace)
        {
            var buildingId = commercialSpace.BuildingId;
            var context = new SphDataContext();

            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            commercialSpace.State = building.Address.State;
            commercialSpace.City = building.Address.City;
            commercialSpace.BuildingName = building.Name;
            commercialSpace.BuildingLot = building.LotNo;

            using (var session = context.OpenSession())
            {
                session.Attach(commercialSpace);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
