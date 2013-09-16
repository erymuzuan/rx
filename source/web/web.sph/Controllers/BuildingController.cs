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

        public async Task<ActionResult> SaveMap(int buildingId, string path)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            item.EncodedWkt = path;
            var points = path.Decode().ToList();
            if (
                Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                )
                points.Add(points.First().Clone());

            item.Wkt = points.ToWkt();

            await spatial.UpdateAsync(item);
            return Json(true);
        }


        public async Task<ActionResult> GetFloorPlan(int id, string floor, string lot)
        {
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = item.FloorCollection.Single(f => f.Name == floor).LotCollection.Single(l => l.Name == lot);
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
            var lotItem = building.FloorCollection.Single(f => f.Name == floorname).LotCollection.Single(l => l.Name == lot);
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
                    Type = typeof(Lot).Name,
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


        public async Task<ActionResult> SaveBuilding(Building building)
        {
            var context = new SphDataContext();
            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == building.BuildingId) ?? building;
            if (item != building)
            {

                item.Address = building.Address;
                item.Name = building.Name;
                item.LotNo = building.LotNo;
                item.Floors = building.Floors;
                item.BuildingSize = building.BuildingSize;
                item.Status = building.Status;
                item.FloorCollection.ClearAndAddRange(building.FloorCollection);
                item.BlockCollection.ClearAndAddRange(building.BlockCollection);
                item.CustomFieldValueCollection.ClearAndAddRange(building.CustomFieldValueCollection);
                item.CustomListValueCollection.ClearAndAddRange(building.CustomListValueCollection);
            }

            var errorMessage = new StringBuilder();
            var duplicateFloors = building.FloorCollection.Select(f => f.Name)
                                 .GroupBy(s => s)
                                 .Any(s => s.Count() > 1);
            if (duplicateFloors)
            {
                errorMessage.AppendLine("There are duplicate floor name or number");
            }

            foreach (var floor in building.FloorCollection)
            {
                var duplicateLot = floor.LotCollection.Select(l => l.Name)
                                        .GroupBy(s => s)
                                        .Any(s => s.Count() > 1);
                if (duplicateLot)
                    errorMessage.AppendLine("There are duplicate Lot in floor" + floor.Name);

            }
            if (errorMessage.Length > 0)
                return Json(new { status = false, message = errorMessage.ToString() });

            using (var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Editing building details");
            }
            return Json(new { status = "success", buildingId = building.BuildingId, message = item.Name });
        }

        public async Task<ActionResult> AddLot(Floor floor, int buildingId, string floorname)
        {
            var duplicateLot = floor.LotCollection.Select(l => l.Name)
                                    .GroupBy(s => s)
                                    .Any(s => s.Count() > 1);
            if (duplicateLot)
                return Json(new { status = false, message = "There are duplicate Lot in floor " + floor.Name });


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
