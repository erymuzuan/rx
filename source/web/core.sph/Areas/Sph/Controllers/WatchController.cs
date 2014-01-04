using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class WatchController : Controller
    {
        public async Task<ActionResult> Register(int id, string entity)
        {
            var context = new SphDataContext();
            var watcher = new Watcher();
            var watch = await context.LoadOneAsync<Watcher>(w => w.EntityId == id) ?? watcher;

            watch.DateTime = DateTime.Now;
            watch.User = User.Identity.Name;
            watch.EntityName = entity;
            watch.EntityId = id;
            watch.IsActive = true;

            using (var session = context.OpenSession())
            {
                session.Attach(watch);
                await session.SubmitChanges("Watcher saved");
            }

            return Json(true);
        }

        public async Task<ActionResult> Deregister(string entity, int id)
        {
            var context = new SphDataContext();
            var watch = await context.LoadOneAsync<Watcher>(w => w.EntityId == id);
            if (null == watch) return Json(false);

            using (var session = context.OpenSession())
            {
                session.Delete(watch);
                await session.SubmitChanges("Watcher deregistered");
            }

            return Json(true);
        }

        public async Task<ActionResult> GetWatch(string entity, int id)
        {
            var context = new SphDataContext();
            var watch = await context.LoadOneAsync<Watcher>(w => w.EntityId == id);
            return Json(null != watch);
        }

    }
}
