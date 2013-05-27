using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class SettingController : Controller
    {
        public async Task<ActionResult> Save(IEnumerable<Setting> settings)
        {
            settings = settings.ToList();

            var keys = settings.Select(s => s.Key).ToArray();
            var context = new SphDataContext();
            var query = context.Settings.Where(s => keys.Contains(s.Key));
            var lo = await context.LoadAsync(query);

            var finals = settings.Union(lo.ItemCollection).Where(s => !string.IsNullOrWhiteSpace(s.Value));

            using (var session = context.OpenSession())
            {
                session.Attach(finals.Cast<Entity>().ToArray());
                await session.SubmitChanges();
            }

            return Json(true);

        } 

    }
}
