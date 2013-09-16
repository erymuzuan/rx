using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult TemplateSpaceHtml()
        {
            return RedirectToAction("Space", "Template");
     
        }
        public ActionResult TemplateApplicationHtml()
        {
            return RedirectToAction("Application", "Template");
     
        }

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
