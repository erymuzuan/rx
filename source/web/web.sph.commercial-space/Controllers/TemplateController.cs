using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TemplateController : Controller
    {

        public async Task<ActionResult> SaveComplaintTemplate()
        {
            var template = this.GetRequestJson<ComplaintTemplate>();
            var models = TypeHelper.GetPropertyPath(typeof(Complaint));
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);

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

            var models = TypeHelper.GetPropertyPath(typeof (Building));
            var template = this.GetRequestJson<BuildingTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.BuildingTemplateId);
        }

        public async Task<ActionResult> SaveCommercialSpaceTemplate()
        {

            var models = TypeHelper.GetPropertyPath(typeof(CommercialSpace));
            var template = this.GetRequestJson<CommercialSpaceTemplate>();
            this.BuildCustomFields(template.CustomFieldCollection, template.FormDesign, models);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges();
            }
            return Json(template.CommercialSpaceTemplateId);
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
            this.BuildCustomFields(template.CustomFieldCollection,template.FormDesign, models);
           
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

        public ActionResult Building()
        {
            var vm = new TemplateFormViewModel { Entity = "building" };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new BuildingFloorsElement());
            vm.FormElements.Add(new BuildingMapElement());


            return View(vm);

        }

        public ActionResult Complaint()
        {
            var vm = new TemplateFormViewModel { Entity = "complaint" };
            vm.FormElements.Add(new AddressElement());
            vm.FormElements.Add(new ComplaintCategoryElement());


            return View(vm);

        }

        public ActionResult Maintenance()
        {
            var vm = new TemplateFormViewModel { Entity = "maintenance" };
            vm.FormElements.Add(new AddressElement());


            return View(vm);
        }
    }
}
