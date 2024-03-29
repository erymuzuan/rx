﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize]
    public class WorkflowMonitorController : Controller
    {
        public async Task<ActionResult> Search(string workflowDefinitionId, DateTime? createdDateFrom, DateTime? createdDateEnd, string state)
        {
            var context = new SphDataContext();
            var query = context.Workflows.Where(w => w.WorkflowDefinitionId == workflowDefinitionId)
                .WhereIf(w => w.CreatedDate >= createdDateFrom.Value, createdDateFrom.HasValue)
                .WhereIf(w => w.CreatedDate <= createdDateEnd.Value.AddHours(23).AddMinutes(59), createdDateEnd.HasValue)
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

        public async Task<ActionResult> Terminate(string[] instancesId)
        {
            var context = new SphDataContext();
            var query = context.Workflows.Where(w => instancesId.Contains(w.Id));

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
            return Json(new { success = true, status = "OK" });

        }

        public async Task<ActionResult> DeployedVersions(string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == id);

            var query = context.AuditTrails.Where(a => a.EntityId == id && a.Type == typeof(WorkflowDefinition).Name);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var logs = new ObjectCollection<AuditTrail>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                logs.AddRange(lo.ItemCollection);
            }
            // add the current version too
            var current = new AuditTrail { EntityId = id, Type = typeof(WorkflowDefinition).Name };
            current.ChangeCollection.Add(new Change { OldValue = wd.Version.ToString(CultureInfo.InvariantCulture), PropertyName = "Version" });
            logs.Add(current);

            var versionLogs = from g in logs
                              where g.ChangeCollection.Any(c => c.PropertyName == "Version")
                              select g;

            var bin = System.IO.Path.GetFullPath(Server.MapPath("~/bin/"));
            var sche = ConfigurationManager.SchedulerPath;
            var subs = ConfigurationManager.SubscriberPath;

            var result = from g in versionLogs
                         let version = g.ChangeCollection.Single(c => c.PropertyName == "Version").OldValue
                         let dll = $"workflows.{id}.{version}.dll"
                select new
                         {
                             Version = version,
                             Running = System.IO.File.Exists(System.IO.Path.Combine(bin, dll)),
                             Web = System.IO.File.Exists(System.IO.Path.Combine(bin, dll)),
                             Subscriber = System.IO.File.Exists(System.IO.Path.Combine(subs, dll)),
                             Scheduler = System.IO.File.Exists(System.IO.Path.Combine(sche, dll))
                         };

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(result.ToArray()));

        }
    }
}