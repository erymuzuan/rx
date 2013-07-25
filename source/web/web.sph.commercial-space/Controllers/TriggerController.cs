using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TriggerController : Controller
    {
        private string GetRequestBody()
        {

            using (var reader = new StreamReader(this.Request.InputStream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }

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
