﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using DiffPlex;
using DiffPlex.DiffBuilder;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class PageController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var page = this.GetRequestJson<Page>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(page);
                await session.SubmitChanges();
            }
            return Json(new { status = "OK", success = true });
        }
        public async Task<ActionResult> Remove()
        {
            var page = this.GetRequestJson<Page>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(page);
                await session.SubmitChanges();
            }
            return Json(new { status = "OK", success = true });
        }

        public async Task<ActionResult> DownloadLog(string id)
        {
            var context = new SphDataContext();
            var log = await context.LoadOneAsync<AuditTrail>(d => d.Id == id);
            if (null == log) return new HttpNotFoundResult("canno find code for log " + id);
            var change = log.ChangeCollection.SingleOrDefault(c => c.PropertyName == "Code");
            if (null == change) return new HttpNotFoundResult("no code for log " + id);

            var page = await context.LoadOneAsync<Page>(p => p.Id == log.EntityId);

            var content = Encoding.UTF8.GetBytes(change.OldValue);
            return File(content, "application/text", System.IO.Path.GetFileName(page.VirtualPath));

        }

        public async Task<ActionResult> Compare(string id)
        {
            var context = new SphDataContext();
            var log = await context.LoadOneAsync<AuditTrail>(d => d.Id == id);
            if (null == log) return new HttpNotFoundResult("canno find code for log " + id);
            var change = log.ChangeCollection.SingleOrDefault(c => c.PropertyName == "Code");
            if (null == change) return new HttpNotFoundResult("no code for log " + id);

            var page = await context.LoadOneAsync<Page>(p => p.Id == log.EntityId);
            var vm = new PageCompareViewModel { Latest = page.Code, Old = change.OldValue, LogId = id };


            var diffBuilder = new SideBySideDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(vm.Old, vm.Latest);
            vm.Diff = diff;
            return View(vm);

        }
    }
}