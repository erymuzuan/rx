using System.Threading.Tasks;
using System.Web.Mvc;
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

            var vm = new PrintViewModel {Item = item, FormDesign = template.FormDesign};
            return View("Index", vm);
        }

    }

    public class PrintViewModel
    {
        public Entity Item { get; set; }

        public FormDesign FormDesign { get; set; }
    }
}
