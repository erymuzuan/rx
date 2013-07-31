﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

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

        public async Task<ActionResult> SaveBuildingTemplate(BuildingTemplate buildingTemplate)
        {
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

        public async Task<ActionResult> Building()
        {
            var vm = new TemplateFormViewModel();
            vm.FormElements.Add(new TextBox());
            vm.FormElements.Add(new ComboBox());
            vm.FormElements.Add(new WebsiteFormElement());
            vm.FormElements.Add(new EmailFormElement());
            vm.FormElements.Add(new NumberTextBox());
            vm.FormElements.Add(new CheckBox());
            vm.FormElements.Add(new TextAreaElement());
            return View(vm);
        }
    }

    public class TemplateFormViewModel
    {
        private readonly ObjectCollection<FormElement> m_nameCollection = new ObjectCollection<FormElement>();

        public ObjectCollection<FormElement> FormElements
        {
            get { return m_nameCollection; }
        }
    }
}
