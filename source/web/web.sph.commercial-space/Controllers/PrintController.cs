using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class PrintController : Controller
    {
        public async Task<ActionResult> Building(int id)
        {
            var context = new SphDataContext();
            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var template = await context.LoadOneAsync<BuildingTemplate>(t => t.BuildingTemplateId == item.TemplateId);
            
            var vm = new PrintViewModel {Name = item.Name, Item = item, FormDesign = template.FormDesign};
            return View("Index", vm);
        }

    }
}
