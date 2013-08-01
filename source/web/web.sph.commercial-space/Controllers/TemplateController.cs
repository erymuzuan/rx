using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TemplateController : Controller
    {

        public async Task<ActionResult> SaveComplaintTemplate(ComplaintTemplate complainttemplate)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(complainttemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveBuildingTemplate()
        {

            var json = this.GetRequestBody();
            var buildingTemplate = JsonConvert.DeserializeObject<BuildingTemplate>(json,
                                                                                   new JsonSerializerSettings
                                                                                       {
                                                                                           TypeNameHandling =
                                                                                               TypeNameHandling.All
                                                                                       });

            var list = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(list, "", typeof(Building));
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();

            if (buildingTemplate.CustomFieldCollection.Any())
            {
                buildingTemplate.CustomFieldCollection.Clear();
            }

            foreach (var el in buildingTemplate.FormDesign.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        buildingTemplate.CustomFieldCollection.Add(cf);
                }
                else
                {
                    el.CustomField = null;
                }
            }

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(buildingTemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveCommercialSpaceTemplate(CommercialSpaceTemplate csTemplate)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(csTemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveApplicationTemplate(ApplicationTemplate template)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveMaintenanceTemplate(MaintenanceTemplate template)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public ActionResult Building()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.Add(new FormElement());
            vm.FormElements.Add(new SectionFormElement());
            vm.FormElements.Add(new TextBox());
            vm.FormElements.Add(new ComboBox());
            vm.FormElements.Add(new WebsiteFormElement());
            vm.FormElements.Add(new EmailFormElement());
            vm.FormElements.Add(new NumberTextBox());
            vm.FormElements.Add(new CheckBox());
            vm.FormElements.Add(new TextAreaElement());
            vm.FormElements.Add(new DatePicker());

            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new BuildingFloorsElement());
            vm.FormElements.Add(new BuildingMapElement());


            return View(vm);

        }
    }
}
