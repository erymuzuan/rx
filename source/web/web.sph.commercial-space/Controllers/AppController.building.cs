using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public ActionResult TemplateBuildingHtml()
        {
            return RedirectToAction("Building", "Template");
     
        }

        public async Task<ActionResult> BuildingDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<BuildingTemplate>(t => t.BuildingTemplateId == templateId);

            return View(template);
        }

    }
}
