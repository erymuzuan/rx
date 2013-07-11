using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class DesignationController : Controller
    {
        public async Task<ActionResult> Save(Designation designation)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(designation);
                await session.SubmitChanges("Add new designation");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(designation));

        }

    }
}
