using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using System.Linq;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class MapController : Controller
    {

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var result = Guid.NewGuid().ToString();
            var model = this.GetRequestJson<CreateMapModel>();
            if (null == model) throw new Exception("Cannot deserialize CreateMapModel");
            Console.WriteLine("BODY " + model);
            
            
            var points = model.EncodedPath.Decode().ToList();
            if (
                Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                )
                points.Add(points.First().Clone());

            var spatial = new SpatialStore
            {
                EncodedWkt = model.EncodedPath,
                Wkt = points.ToWkt(),
                Type = model.Type,
                Tag = model.Tag,
                StoreId = result
            };
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(spatial);
                await session.SubmitChanges("Create New");
            }

            var repos = ObjectBuilder.GetObject<ISpatialService<SpatialStore>>();
            await repos.UpdateAsync(spatial);


            if (Request.ContentType.Contains("application/json"))
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(result));
            }

            return View(result);
        }
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

            var lot = floor.UnitCollection.SingleOrDefault(l => cs.UnitNo.Contains(l.No));
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
