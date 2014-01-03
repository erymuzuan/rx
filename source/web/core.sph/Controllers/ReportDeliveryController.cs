using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class ReportDeliveryController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var rd = this.GetRequestJson<ReportDelivery>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(rd);
                await session.SubmitChanges("Save");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(rd.ReportDeliveryId));

        }

    }
}
