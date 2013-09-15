using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class CustomFieldController : Controller
    {
        public async Task<ActionResult> BuildingTemplate(int id)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<BuildingTemplate>(t => t.BuildingTemplateId == id);
            return View("CustomFieldHtml", new CustomFieldViewModel(template.CustomFieldCollection){RootObject = "$root.building()"});
        }

        public async Task<ActionResult> ComplaintTemplate(int id)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == id);
            return View("CustomFieldHtml", new CustomFieldViewModel(template.CustomFieldCollection) { RootObject = "$root.complaint()" });
        }
        public async Task<ActionResult> CommercialSpaceTemplate(int id)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<SpaceTemplate>(t => t.CommercialSpaceTemplateId == id);
            return View("CustomFieldHtml", new CustomFieldViewModel(template.CustomFieldCollection) { RootObject = "$root.complaint()" });
        }
    }
}
