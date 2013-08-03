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

        public async Task<ActionResult> SaveComplaintTemplate()
        {
            var json = this.GetRequestBody();
            var complainttemplate = JsonConvert.DeserializeObject<ComplaintTemplate>(json,
                                                                                   new JsonSerializerSettings
                                                                                   {
                                                                                       TypeNameHandling =
                                                                                           TypeNameHandling.All
                                                                                   });

            var list = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(list, "", typeof(Complaint));
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();

            if (complainttemplate.CustomFieldCollection.Any())
            {
                complainttemplate.CustomFieldCollection.Clear();
            }

            foreach (var el in complainttemplate.FormDesign.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        complainttemplate.CustomFieldCollection.Add(cf);
                }
                else
                {
                    el.CustomField = null;
                }
            }
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

        public async Task<ActionResult> SaveCommercialSpaceTemplate()
        {
            var json = this.GetRequestBody();
            var csTemplate = JsonConvert.DeserializeObject<CommercialSpaceTemplate>(json,
                                                                                   new JsonSerializerSettings
                                                                                   {
                                                                                       TypeNameHandling =
                                                                                           TypeNameHandling.All
                                                                                   });

            var list = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(list, "", typeof(Maintenance));
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();

            if (csTemplate.CustomFieldCollection.Any())
            {
                csTemplate.CustomFieldCollection.Clear();
            }

            foreach (var el in csTemplate.FormDesign.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        csTemplate.CustomFieldCollection.Add(cf);
                }
                else
                {
                    el.CustomField = null;
                }
            }
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(csTemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveApplicationTemplate()
        {
            var json = this.GetRequestBody();
            var apptemplate = JsonConvert.DeserializeObject<ApplicationTemplate>(json,
                                                                                   new JsonSerializerSettings
                                                                                   {
                                                                                       TypeNameHandling =
                                                                                           TypeNameHandling.All
                                                                                   });

            var list = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(list, "", typeof(RentalApplication));
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();

            if (apptemplate.CustomFieldCollection.Any())
            {
                apptemplate.CustomFieldCollection.Clear();
            }

            foreach (var el in apptemplate.FormDesign.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        apptemplate.CustomFieldCollection.Add(cf);
                }
                else
                {
                    el.CustomField = null;
                }
            }
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(apptemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> SaveMaintenanceTemplate()
        {
            var json = this.GetRequestBody();
            var maintenanceTemplate = JsonConvert.DeserializeObject<MaintenanceTemplate>(json,
                                                                                   new JsonSerializerSettings
                                                                                   {
                                                                                       TypeNameHandling =
                                                                                           TypeNameHandling.All
                                                                                   });

            var list = new List<string>();
            TypeHelper.BuildFlatJsonTreeView(list, "", typeof(Maintenance));
            var tjson = "[" + string.Join(",", list) + "]";
            var models = JsonConvert.DeserializeObject<IEnumerable<TypeModel>>(tjson)
                                    .Select(t => t.Path)
                                    .ToArray();

            if (maintenanceTemplate.CustomFieldCollection.Any())
            {
                maintenanceTemplate.CustomFieldCollection.Clear();
            }

            foreach (var el in maintenanceTemplate.FormDesign.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        maintenanceTemplate.CustomFieldCollection.Add(cf);
                }
                else
                {
                    el.CustomField = null;
                }
            }
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(maintenanceTemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public ActionResult Building()
        {
            var vm = new TemplateFormViewModel { Entity = "building" };
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

        public ActionResult Complaint()
        {
            var vm = new TemplateFormViewModel { Entity = "complaint" };
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


            return View(vm);

        }

        public ActionResult Maintenance()
        {
            var vm = new TemplateFormViewModel { Entity = "maintenance" };
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


            return View(vm);
        }
    }
}
