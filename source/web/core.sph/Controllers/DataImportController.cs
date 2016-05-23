using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "administrators,developers")]
    [RoutePrefix("api/data-imports")]
    public class DataImportController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save(DataTransferDefinition model)
        {
            if (model.IsNewItem)
                model.Id = model.Name.ToIdFormat();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(model);
                await session.SubmitChanges();
            }
            return Json(new { });
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