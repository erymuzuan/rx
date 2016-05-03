using System.Linq;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    [RoutePrefix("api/data-imports")]
    public class DataImportController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public IHttpActionResult Save(ImportDataViewModel model)
        {
            var folder = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);


            var setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented, setting);
            System.IO.File.WriteAllText($"{ConfigurationManager.WebPath}/App_Data/data-imports/{model.Name}.json", json);
            return Json(new { });
        }

        [HttpDelete]
        [Route("{name}")]
        public IHttpActionResult Remove(string name)
        {
            var file = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/{name}.json");
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            return Json(new { });
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult List()
        {
            var folder = $"{ConfigurationManager.WebPath}/App_Data/data-imports/";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            var files = from f in System.IO.Directory.GetFiles(folder, "*.json")
                        select System.IO.File.ReadAllText(f);

            return Json("[" + string.Join(",", files.ToArray()) + "]");
        }


    }
}