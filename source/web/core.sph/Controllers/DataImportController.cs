using System;
using System.Linq;
using System.Text;
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
        [HttpGet]
        [Route("{model}/errors")]
        public IHttpActionResult ErrorList(string model, [FromUri(Name = "$take")]int take = 20, [FromUri(Name = "$skip")]int skip = 0)
        {
            var folder = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\{model.ToIdFormat()}";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            var files = System.IO.Directory.GetFiles(folder, "*.data");
            var rows = from f in files.Skip(skip).Take(take)
                let id = System.IO.Path.GetFileNameWithoutExtension(f)
                let errorFile = $"{folder}\\{id}.error"
                let data = System.IO.File.ReadAllText(f)
                let error = System.IO.File.ReadAllText(errorFile)
                let lines = error.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                let errorJson = JsonConvert.SerializeObject(new
                {
                    Message = lines[3],
                    Type = lines[2],
                    StackTrace = string.Join("\r\n", lines.Skip(4)),
                    Details = error
                })
                select $@"
{{
    ""ErrorId"" : ""{model.ToIdFormat()}/{id}"",
    ""Data"" : {data},
    ""Exception"" : {errorJson}
}}";

            var sb = new StringBuilder();
            sb.Append(@"{ 
""items"" : [");
            sb.Append(string.Join(",", rows));
            sb.Append($@"],
                ""total"" : {files.Length}
}}");

            return Json(sb.ToString());
        }


    }
}