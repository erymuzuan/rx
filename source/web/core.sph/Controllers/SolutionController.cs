using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    public class SolutionController : Controller
    {
        [Route("open/{path}")]
        [HttpGet]
        public async Task<ActionResult> OpenAsync(string path)
        {
            var solution = await Solution.LoadAsync(path);
            var json = solution.ToJsonString(true);
            this.Response.ContentType = "application/json";
            return Content( json);

        }

        [Route("createsolution")]
        [HttpPost]
        public async Task<ActionResult> NewSolution()
        {
            // check if i can write to that directory

            // create name.json file       
            JObject obj = new JObject();
            obj["ApplicationName"] = "rxhassanapplicationname";
            obj["ProjectDirectory"] = "C:\\project\\work\\rxhassan";
            obj["IISPort"] = "52310";
            obj["IISDirectory"] = "C:\\Program Files (x86)\\IIS Express\\iisexpress.exe";
            obj["LocalDBInstanceName"] = "v11.0";
            obj["RabbitMQDirectory"] = "rabbitmq_server";
            obj["RabbitMQUsername"] = "guest";
            obj["RabbitMQPassword"] = "guest";
            obj["JavaHome"] = "C:\\Program Files (x86)\\Java\\jre1.8.0_25";
            obj["ElasticsearchHome"] = "elasticsearch";

            // write to .json file
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Solution.SaveNewSolution(json,obj);

            return Json(new { success = "true" });

        }
    }
}
