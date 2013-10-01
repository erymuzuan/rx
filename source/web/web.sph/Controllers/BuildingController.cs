using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class BuildingController : Controller
    {
        public async Task<ActionResult> GetCenter(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var center = await spatial.GetCenterAsync(b => b.BuildingId == id);
            if (null == center) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(center, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEncodedPath(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var encodedPath = await spatial.GetEncodedPathAsync(b => b.BuildingId == id);
            if (null == encodedPath) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(encodedPath, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveMap(int buildingId, string path, LatLng point)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var context = new SphDataContext();

            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);

            if (null != point)
            {
                building.EncodedWkt = GoogleMapHelper.EncodeLatLong(new[] { point });
                building.Wkt = point.ToWKT();
            }

            if (!string.IsNullOrWhiteSpace(path))
            {

                building.EncodedWkt = path;
                var points = path.Decode().ToList();
                if (
                    Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                    || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                    )
                    points.Add(points.First().Clone());

                building.Wkt = points.ToWkt();
            }

            await spatial.UpdateAsync(building);
            return Json(true);
        }


        public async Task<ActionResult> GetFloorPlan(int id, string floor, string lot)
        {
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = item.FloorCollection.Single(f => f.Name == floor).UnitCollection.Single(l => l.No == lot);
            if (string.IsNullOrWhiteSpace(lotItem.PlanStoreId)) return Json(new { EncodedPolygon = string.Empty }, JsonRequestBehavior.AllowGet);
            var line = await context.GetScalarAsync<SpatialStore, string>(s => s.StoreId == lotItem.PlanStoreId, s => s.EncodedWkt);

            var shape = new
                {
                    EncodedPolygon = line,
                    lotItem.FillColor,
                    lotItem.FillOpacity
                };

            return Json(shape, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddLotFloorPlan(int id, string floorname, string lot, string fillColor, double fillOpacity, string path)
        {
            var context = new SphDataContext();
            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = building.FloorCollection.Single(f => f.Name == floorname).UnitCollection.Single(l => l.No == lot);
            lotItem.FillOpacity = fillOpacity;
            lotItem.FillColor = fillColor;
            lotItem.PlanStoreId = Guid.NewGuid().ToString();

            var points = path.Decode().ToList();
            if (
                Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                )
                points.Add(points.First().Clone());


            var store = new SpatialStore
                {
                    StoreId = lotItem.PlanStoreId,
                    Type = typeof(Unit).Name,
                    Tag = string.Format("{0};{1}{2}", id, floorname, lot),
                    EncodedWkt = path,
                    Wkt = points.ToWkt()
                };

            using (var session = context.OpenSession())
            {
                session.Attach(store, building);
                await session.SubmitChanges();
            }

            var spatial = ObjectBuilder.GetObject<ISpatialService<SpatialStore>>();
            await spatial.UpdateAsync(store);

            return Json(true);
        }

        public async Task<ActionResult> Remove()
        {
            var building = this.GetRequestJson<Building>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(building);
                await session.SubmitChanges("Remove");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(new { status = "OK" }));

        }


        public async Task<ActionResult> Save()
        {
            var building = this.GetRequestJson<Building>();
            var context = new SphDataContext();


            var errorMessage = new StringBuilder();
            var duplicateBlocks = building.BlockCollection.Select(b => b.Name)
                                    .GroupBy(b => b)
                                    .Any(b => b.Count() > 1);
            if (duplicateBlocks)
            {
                errorMessage.AppendLine("There are duplicate block name");
            }

            foreach (var block in building.BlockCollection)
            {
                var duplicateFloors = block.FloorCollection.Select(f => f.Name)
                                    .GroupBy(f => f)
                                    .Any(f => f.Count() > 1);
                if (duplicateFloors)
                {
                    errorMessage.AppendLine("There are duplicate floor in this block");
                }

                foreach (var floor in block.FloorCollection)
                {
                    var duplicateLot = floor.UnitCollection.Select(l => l.No)
                                            .GroupBy(s => s)
                                            .Any(s => s.Count() > 1);
                    if (duplicateLot)
                        errorMessage.AppendLine("There are duplicate Unit in floor" + floor.Name);

                }
            }
            
            if (errorMessage.Length > 0)
                return Json(new { status = false, message = errorMessage.ToString() });

            using (var session = context.OpenSession())
            {
                session.Attach(building);
                await session.SubmitChanges("Editing building details");
            }
            return Json(new { status = "success", buildingId = building.BuildingId, message = building.Name });
        }

        public async Task<ActionResult> AddLot(Floor floor, int buildingId, string floorname)
        {
            var duplicateLot = floor.UnitCollection.Select(l => l.No)
                                    .GroupBy(s => s)
                                    .Any(s => s.Count() > 1);
            if (duplicateLot)
                return Json(new { status = false, message = "There are duplicate Unit in floor " + floor.Name });


            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            var dbfloor = dbItem.FloorCollection.Single(f => f.Name == floorname);

            dbItem.BuildingId = buildingId;
            dbItem.FloorCollection.Replace(dbfloor, floor);



            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }
            return Json(new { status = "success", message = "Your floor details has been saved" });
        }
    }
}
