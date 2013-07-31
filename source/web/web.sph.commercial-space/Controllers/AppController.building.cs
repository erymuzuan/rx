using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public ActionResult BuildingTemplateFormHtml()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.Add(new TextBox());
            vm.FormElements.Add(new ComboBox());
            vm.FormElements.Add(new WebsiteFormElement());
            vm.FormElements.Add(new EmailFormElement());
            vm.FormElements.Add(new NumberTextBox());
            vm.FormElements.Add(new CheckBox());
            vm.FormElements.Add(new TextAreaElement());
            vm.FormElements.Add(new DatePicker());
            return View("../Template/Building", vm);
        }

        public async Task<ActionResult> BuildingDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<BuildingTemplate>(t => t.BuildingTemplateId == templateId);

            return View(template.CustomFieldCollection);
        }

    }
}
