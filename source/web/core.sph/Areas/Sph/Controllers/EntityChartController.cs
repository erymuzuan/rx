
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityChartController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var chart = this.GetRequestJson<EntityChart>();
            var context = new SphDataContext();
            if (chart.IsNewItem)
                chart.Id = Guid.NewGuid().ToString();

            using (var session = context.OpenSession())
            {
                session.Attach(chart);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = chart.Id });
        }


        public async Task<ActionResult> Remove()
        {
            var chart = this.GetRequestJson<EntityChart>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(chart);
                await session.SubmitChanges("Delete");
            }
            return Json(new { success = true, status = "OK", id = chart.Id });
        }
	}
}