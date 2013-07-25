using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TriggerController : Controller
    {

        public async Task<ActionResult> Save()
        {
            var json = this.GetRequestBody();
            var trigger = JsonConvert.DeserializeObject<Trigger>(json);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(trigger);
                await session.SubmitChanges("Submit trigger");
            }


            return Json(trigger.TriggerId);
        }

    }
}
