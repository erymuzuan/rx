using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class MapController : Controller
    {
        public async Task<ActionResult> Get(string id, string[] filter)
        {
            var bounds = GoogleMapHelper.ParseBound(id).ToArray();

            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var buildings = await spatial.ContainsAsync(b => b.Status == "Active", bounds);
            var list = from b in buildings
                       select new
                           {
                               b.Name,
                               b.BuildingId,
                               b.Address,
                               b.EncodedWkt,
                               b.Size
                           };
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CommercialSpaceImage(int id, int width = 400, int height = 400)
        {
            var context = new SphDataContext();
            var cs = await context.LoadOneAsync<CommercialSpace>(c => c.CommercialSpaceId == id);
            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == cs.BuildingId);
            var lot =
                building.FloorCollection.Single(f => f.Name == cs.FloorName)
                        .LotCollection.Single(l => l.Name == cs.LotName);

            var item = await context.GetScalarAsync<SpatialStore, string>(b => b.StoreId == lot.PlanStoreId, s => s.EncodedWkt);

            var url = string.Format(
                    "http://maps.googleapis.com/maps/api/staticmap?size={1}x{2}&&path=fillcolor:0xAA000033%7Ccolor:0xFFFFFF00%7Cenc:{0}&sensor=false", item, width, height);
            return Content(url);
        }

    }
}
