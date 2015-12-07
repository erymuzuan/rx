using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("developer-service")]
    public class DeveloperServiceController : Controller
    {
        [Route("websocket-port")]
        public ActionResult GetDeveloperLogPortNumber()
        {
            var file = System.IO.Path.Combine(ConfigurationManager.BaseDirectory, "project.json");
            if (System.IO.File.Exists(file))
            {
                var json = JObject.Parse(System.IO.File.ReadAllText(file));
                var port = json.SelectToken("$.loggerWebSocketPort").Value<int>();
                return Content(port.ToString());
            }
            return Content("50238");
        }
    }
}