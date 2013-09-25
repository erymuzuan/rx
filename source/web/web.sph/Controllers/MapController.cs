using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using System.Linq;

namespace Bespoke.Sph.Web.Controllers
{
    public class MapController : Controller
    {
        public async Task<ActionResult> Get(string id, string[] filter)
        {
            var bounds = GoogleMapHelper.ParseBound(id).ToArray();

            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var buildings = await spatial.ContainsAsync(b => b.Status == "Active", bounds);
            var list = from b in buildings
                       let point = b.EncodedWkt.Decode().First()
                       let photo = b.PhotoCollection.SingleOrDefault(p => p.Title == "MapImage")
                       select new
                           {
                               b.Name,
                               b.BuildingId,
                               b.Address,
                               EncodedWkt = GoogleMapHelper.EncodeLatLong(new List<LatLng> { point }),
                               b.BuildingSize,
                               b.Floors,
                               b.TemplateId,
                               b.TemplateName,
                               MapImage = photo != null ? photo.StoreId : "no-exisit",
                               MapIcon = "/images/maps/office-building.png"
                           };
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SpaceImage(int id, int width = 400, int height = 400)
        {
            var context = new SphDataContext();
            var cs = await context.LoadOneAsync<Space>(c => c.SpaceId == id);
            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == cs.BuildingId);
            if (null == building)
                return Content("/images/no-image.png");
            var floor = building.FloorCollection.SingleOrDefault(f => f.Name == cs.FloorName);
            if (null == floor)
                return Content("/images/no-image.png");

            var lot = floor.LotCollection.SingleOrDefault(l => cs.LotName.Contains(l.Name));
            if (null == lot)
                return Content("/images/no-image.png");

            var item = await context.GetScalarAsync<SpatialStore, string>(b => b.StoreId == lot.PlanStoreId, s => s.EncodedWkt);
            if (string.IsNullOrWhiteSpace(item))
                return Content("/images/no-image.png");

            var url = string.Format(
                    "http://maps.googleapis.com/maps/api/staticmap?size={1}x{2}&&path=fillcolor:0xAA000033%7Ccolor:0xFFFFFF00%7Cenc:{0}&sensor=false", item, width, height);
            return Content(url);
        }

    }
}
