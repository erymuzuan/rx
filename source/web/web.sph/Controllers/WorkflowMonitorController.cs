using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    public class WorkflowMonitorController : Controller
    {
        public async Task<ActionResult> Search(int workflowDefinitionId, DateTime? createdDateStart, DateTime? createdDateEnd, string state)
        {
            var context = new SphDataContext();
            var query = context.Workflows.Where(w => w.WorkflowDefinitionId == workflowDefinitionId)
                .WhereIf(w => w.CreatedDate >= createdDateStart.Value, createdDateStart.HasValue)
                .WhereIf(w => w.CreatedDate >= createdDateEnd.Value, createdDateEnd.HasValue)
                .WhereIf(w => w.State == state, !string.IsNullOrWhiteSpace(state));


            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var result = new ObjectCollection<Workflow>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                result.AddRange(lo.ItemCollection);
            }

            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(result.ToArray(), setting));

        }
        public async Task<ActionResult> Terminate(int[] instancesId)
        {
            var context = new SphDataContext();
            var query = context.Workflows.Where(w => instancesId.Contains(w.WorkflowId));

            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var result = new ObjectCollection<Workflow>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                result.AddRange(lo.ItemCollection);
            }

            foreach (var wf in result)
            {
                await wf.TerminateAsync();
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new{ success = true, status = "OK"});

        }
    }
}