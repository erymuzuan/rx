using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.Domain;
using System.Linq;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class TemplateController : Controller
    {

        public async Task<ActionResult> SaveComplaintTemplate()
        {
            var template = this.GetRequestJson<ComplaintTemplate>();
            var models = TypeHelper.GetPropertyPath(typeof(Complaint));
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);
            var list = new ObjectCollection<CustomListDefinition>();
            foreach (var c in template.FormDesign.FormElementCollection.OfType<CustomListDefinitionElement>())
            {
                var cl = new CustomListDefinition
                {
                    Name = c.Name
                };
                cl.CustomFieldCollection.AddRange(c.CustomFieldCollection);
                list.Add(cl);
            }


            template.CustomListDefinitionCollection.ClearAndAddRange(list);

            var errors = (await template.ValidateAsync()).ToList();
            if (errors.Any())
            {
                return Json(new { status = "ERROR", message = "There's are errors", errors = errors.ToArray() });
            }

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.ComplaintTemplateId);
        }

        public async Task<ActionResult> SaveBuildingTemplate()
        {
            var models = TypeHelper.GetPropertyPath(typeof(Building));
            var template = this.GetRequestJson<BuildingTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);
            var list = new ObjectCollection<CustomListDefinition>();
            foreach (var c in template.FormDesign.FormElementCollection.OfType<CustomListDefinitionElement>())
            {
                var cl = new CustomListDefinition
                {
                    Name = c.Name
                };
                cl.CustomFieldCollection.AddRange(c.CustomFieldCollection);
                list.Add(cl);
            }


            template.CustomListDefinitionCollection.ClearAndAddRange(list);

            var errors = (await template.ValidateAsync()).ToList();
            if (errors.Any())
            {
                return Json(new { status = "ERROR", message = "There's are errors", errors = errors.ToArray() });
            }

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(new { status = "OK", id = template.BuildingTemplateId, message = "template was succesfully saved" });
        }

        public async Task<ActionResult> RemoveBuildingTemplate()
        {
            var template = this.GetRequestJson<BuildingTemplate>();
            var context = new SphDataContext();
            var existBuilding = await context.GetAnyAsync<Building>(b => b.TemplateId == template.BuildingTemplateId);
            if (existBuilding)
                return Json(new { status = "ERROR", message = "There are buildings for this template" });

            using (var session = context.OpenSession())
            {
                session.Delete(template);
                await session.SubmitChanges();
            }
            return Json(new { status = "OK", message = "template was succesfully deleted" });
        }

        public async Task<ActionResult> SaveSpaceTemplate()
        {

            var models = TypeHelper.GetPropertyPath(typeof(Space));
            var template = this.GetRequestJson<SpaceTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);
            var list = new ObjectCollection<CustomListDefinition>();
            foreach (var c in template.FormDesign.FormElementCollection.OfType<CustomListDefinitionElement>())
            {
                var cl = new CustomListDefinition
                {
                    Name = c.Name
                };
                cl.CustomFieldCollection.AddRange(c.CustomFieldCollection);
                list.Add(cl);
            }


            template.CustomListDefinitionCollection.ClearAndAddRange(list);
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.SpaceTemplateId);
        }

        public async Task<ActionResult> SaveApplicationTemplate()
        {

            var models = TypeHelper.GetPropertyPath(typeof(RentalApplication));
            var template = this.GetRequestJson<ApplicationTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.ApplicationTemplateId);
        }

        public async Task<ActionResult> SaveMaintenanceTemplate()
        {

            var models = TypeHelper.GetPropertyPath(typeof(Maintenance));
            var template = this.GetRequestJson<MaintenanceTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.MaintenanceTemplateId);
        }


        private void BuildCustomFields(ObjectCollection<CustomField> fields, FormDesign form, string[] models)
        {
            if (fields.Any())
            {
                fields.Clear();
            }

            foreach (var el in form.FormElementCollection)
            {
                if (!models.Contains(el.Path))
                {
                    var cf = el.GenerateCustomField();
                    if (null != cf)
                        fields.Add(cf);
                    el.CustomField = cf;
                }
                else
                {
                    el.CustomField = null;
                }
            }
        }

        public ActionResult Application()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(RentalApplication).Name };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new RentalApplicationAttachmentsElement());
            vm.FormElements.Add(new RentalApplicationBanksElement());
            vm.FormElements.Add(new RentalApplicationContactElement());
            return View(vm);

        }
        public ActionResult Building()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Building).Name };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new BuildingFloorsElement());
            vm.FormElements.Add(new BuildingBlocksElement());
            vm.FormElements.Add(new BuildingMapElement());


            return View(vm);

        }

        public ActionResult Complaint()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Complaint).Name };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new ComplaintCategoryElement());


            return View(vm);

        }

        public ActionResult Space()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Space).Name };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new BuildingElement());
            vm.FormElements.Add(new SpaceLotsElement());

            return View(vm);

        }

        public ActionResult Maintenance()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Maintenance).Name };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new MaintenanceOfficerElement());


            return View(vm);
        }

        public async Task<ActionResult> PropertyPath(string id)
        {
            var dataSource = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = (typeof(Entity).GetShortAssemblyQualifiedName() ?? "").Replace("Entity", id);
            var cols =( await dataSource.GetColumnsAsync(Type.GetType(typeName)))
                .Where(a =>! a.IsCustomField);

            var json = JsonConvert.SerializeObject(cols);

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(json);

        }
    }
}
