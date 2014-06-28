using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityFormRendererController : BaseSphController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == form.EntityDefinitionId);
            
            var layout = form.Layout ?? "Html2ColsWithAuditTrail";
            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };


            return View(layout, vm);
        }

        [RazorScriptFilter]
        public async Task<ActionResult> Js(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.EntityDefinitionId == form.EntityDefinitionId);

            var vm = new FormRendererViewModel { Form = form, EntityDefinition = ed };
            this.Response.ContentType = "application/javascript";
            return Script("Script", vm);


        }



    }
}