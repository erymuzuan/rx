using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class EntityFormRendererController : BaseAppController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            return View(form);
        }
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);

            this.Response.ContentType = "application/javascript";
            var script = this.RenderRazorViewToJs("Script", form);
            return Content(script);


        }

        public async Task<ActionResult> Script(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);

            return View(form);
        }
    }
}