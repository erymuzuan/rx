using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class BuildingController : Controller
    {

        public async Task<ActionResult> SaveMap(int buildingId, string path)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            item.EncodedWkt = path;
            item.Wkt = path.Decode().ToWkt();
            
            await spatial.UpdateAsync(item);
            return Json(true);
        }
       
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
