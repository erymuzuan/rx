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
    public class DeveloperServiceController : BaseController
    {
        [Route("environment-variables")]
        public ActionResult GetEnvironmentVariables()
        {
            var json = JsonConvert.SerializeObject(Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User));
            return Content(json, APPLICATION_JSON, Encoding.UTF8);
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
            var json = JsonConvert.SerializeObject(configs.ToDictionary(k => k.Key, v => v.Value));
            return Content(json, "application/json", Encoding.UTF8);
        }


    }
}