using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> AddLot(Floor floor,int buildingId,string floorname)
        {
            await Task.Delay(5000);
            var context = new SphDataContext();
            var dbItem =await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            var dbfloor = dbItem.FloorCollection.Single(f => f.Name == floorname);

            dbItem.BuildingId = buildingId;
            dbItem.FloorCollection.Replace(dbfloor,floor); 

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }
            return Json(true);
        }
    }
}
