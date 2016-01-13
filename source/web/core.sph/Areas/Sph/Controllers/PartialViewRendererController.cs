using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class PartialViewRendererController : BaseSphController
    {
        public async Task<ActionResult> Html(string id)
        {
            var context = new SphDataContext();
            var view = await context.LoadOneAsync<PartialView>(f => f.Route == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(f => f.Id == view.Entity);

            var vm = new PartialViewRendererViewModel { View= view, EntityDefinition = ed };
            return View(vm);
        }
        
        
    }
}