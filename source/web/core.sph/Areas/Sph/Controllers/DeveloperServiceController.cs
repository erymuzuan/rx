using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("developer-service")]
    public class DeveloperServiceController : Controller
    {
        [Route("websocket-port")]
        public ActionResult GetDeveloperLogPortNumber()
        {
            var port = Environment.GetEnvironmentVariable("loggerWebSocketPort") ?? "50238";
            return Content(port);
        }
        [Route("environment-variables")]
        public ActionResult GetEnvironmentVariables()
        {
            return Json(Environment.GetEnvironmentVariables(), JsonRequestBehavior.AllowGet);
        }

        [Route("configs")]
        public ActionResult GetConfigs()
        {
            var type = typeof(ConfigurationManager);
            var configs = from m in type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                          where m.Name != "ConnectionStrings"
                          && m.Name != "AppSettings"
                          select new
                          {
                              Key = m.Name,
                              Value = m.GetValue(null, null)
                          };
            return Content(JsonConvert.SerializeObject(configs.ToDictionary(k => k.Key, v => v.Value)), "application/json", Encoding.UTF8);
        }


    }
}