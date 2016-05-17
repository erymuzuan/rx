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
            System.IO.File.WriteAllText($"{ConfigurationManager.WebPath}/App_Data/data-imports/{model.Id}.json", json);
            return Json(new { });
        }
        [HttpPost]
        [Route("{id}/schedules")]
        public IHttpActionResult SaveSchedules(string id, IntervalSchedule[] schedules)
        {
            var folder = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            var json = "[" + schedules.JoinString(",", x => x.ToJsonString(true)) + "]";
            System.IO.File.WriteAllText($"{ConfigurationManager.WebPath}/App_Data/data-imports/{id}.schedules.json", json);
            return Json(new { });
        }

        [HttpGet]
        [Route("{id}/schedules")]
        public IHttpActionResult GetSchedules(string id)
        {
            var folder = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/");
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);


            var file = $"{ConfigurationManager.WebPath}/App_Data/data-imports/{id}.schedules.json";
            var json = "[]";
            if (System.IO.File.Exists(file)) json = System.IO.File.ReadAllText(file);
            return Json(json);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Remove(string id)
        {
            var file = ($"{ConfigurationManager.WebPath}/App_Data/data-imports/{id}.json");
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
                        where !f.EndsWith(".schedules.json")
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
                       let lines = error.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
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

        [HttpGet]
        [Route("{model}/histories")]
        public IHttpActionResult GetHistoryList(string model, [FromUri(Name = "$take")]int take = 20, [FromUri(Name = "$skip")]int skip = 0)
        {
            var folder = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\history\\";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            var files = System.IO.Directory.GetFiles(folder, $"{model.ToIdFormat()}-*.log");
            var logs = from f in files.Skip(skip).Take(take)
                       orderby f descending
                       select System.IO.File.ReadAllText(f);

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"items\" : [");
            sb.AppendLine(string.Join(",", logs));
            sb.AppendLine("],");
            sb.AppendLine($"\"total\":{files.Length}");
            sb.Append("}");

            return Json(sb.ToString());
        }


    }
}