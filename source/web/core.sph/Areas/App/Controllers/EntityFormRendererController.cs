using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class EntityFormRendererController : BaseAppController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == form.EntityDefinitionId);

            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };


            return View(vm);
        }
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == form.EntityDefinitionId);

            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };
            this.Response.ContentType = "application/javascript";
            var script = this.RenderRazorViewToJs("Script", vm);
            return Content(script);


        }

        public async Task<ActionResult> Script(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == form.EntityDefinitionId);
            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };

            return View(vm);
        }
    }
}