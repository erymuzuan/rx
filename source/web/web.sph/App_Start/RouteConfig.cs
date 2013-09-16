using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HotTowel", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
// ReSharper disable RedundantBoolCompare
            var buildingTemplatesTask =  context.LoadAsync(context.BuildingTemplates.Where(t => t.IsActive == true));
            var complaintTemplatesTask =  context.LoadAsync(context.ComplaintTemplates.Where(t => t.IsActive == true));
            var ruangTemplatesTask =  context.LoadAsync(context.SpaceTemplates.Where(t => t.IsActive == true));
            var applicationTemplateTask =  context.LoadAsync(context.ApplicationTemplates.Where(t => t.IsActive == true));
            var maintenanceTemplateTask =  context.LoadAsync(context.MaintenanceTemplates.Where(t => t.IsActive == true));
            var rdlTask =  context.LoadAsync(context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user)));
// ReSharper restore RedundantBoolCompare
            await
                Task.WhenAll(buildingTemplatesTask, complaintTemplatesTask, complaintTemplatesTask, ruangTemplatesTask,
                             applicationTemplateTask, maintenanceTemplateTask, rdlTask);

            var buildingTemplates = await buildingTemplatesTask;
            var complaintTemplates = await complaintTemplatesTask;
            var ruangTemplates = await ruangTemplatesTask;
            var applicationTemplates = await applicationTemplateTask;
            var maintenanceTemplates = await maintenanceTemplateTask;
            var rdls = await rdlTask;

            var routes = new List<JsRoute>();
            var buildingRoute = from t in buildingTemplates.ItemCollection
                select new JsRoute
                {
                    Name = t.Name,
                    Url = string.Format("building.detail-templateid.{0}/:templateId/:id", t.BuildingTemplateId),
                    Role = "can_add_space",
                    Caption = t.Name,
                    Icon = "icon-building",
                    ModuleId = string.Format("viewmodels/building.detail-templateid.{0}", t.BuildingTemplateId)
                };

            var csRoute = from t in ruangTemplates.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Name,
                                    Url = string.Format("space.detail-templateid.{0}/:templateId/:buildingId/:floorname/:csId", t.SpaceTemplateId),
                                    Role = "can_edit_space_template",
                                    Caption = t.Name,
                                    Icon = "icon-building",
                                    ModuleId = string.Format("viewmodels/space.detail-templateid.{0}", t.SpaceTemplateId)
                                };
            var complaintFormRoutes = from t in complaintTemplates.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Name,
                                    Url = string.Format("complaint.form-templateid.{0}/:templateId", t.ComplaintTemplateId),
                                    Role = "can_edit_space_template",
                                    Caption = t.Name,
                                    Icon = "icon-building",
                                    ModuleId = string.Format("viewmodels/complaint.form-templateid.{0}", t.ComplaintTemplateId)
                                };
            var complaintAssignmentRoutes = from t in complaintTemplates.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Name,
                                    Url = string.Format("complaint.assign-templateid.{0}/:id", t.ComplaintTemplateId),
                                    Role = "can_assign_complaint",
                                    Caption = t.Name,
                                    Icon = "icon-check",
                                    ModuleId = string.Format("viewmodels/complaint.assign-templateid.{0}", t.ComplaintTemplateId)
                                };
            var complaintCloseRoutes = from t in complaintTemplates.ItemCollection
                                            select new JsRoute
                                            {
                                                Name = t.Name,
                                                Url = string.Format("complaint.close-templateid.{0}/:id", t.ComplaintTemplateId),
                                                Role = "can_assign_complaint",
                                                Caption = t.Name,
                                                Icon = "icon-check",
                                                ModuleId = string.Format("viewmodels/complaint.close-templateid.{0}", t.ComplaintTemplateId)
                                            };
            var applicationRoutes = from t in applicationTemplates.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Name,
                                    Url = string.Format("application.detail-templateid.{0}/:id", t.ApplicationTemplateId),
                                    Caption = t.Name,
                                    Icon = "icon-building",
                                    ModuleId = string.Format("viewmodels/application.detail-templateid.{0}", t.ApplicationTemplateId)
                                };

            var maintenanceRoute = from t in maintenanceTemplates.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Name,
                                    Url = string.Format("maintenance.detail-templateid.{0}/:templateId/:id", t.MaintenanceTemplateId),
                                    Caption = t.Name,
                                    Icon = "icon-building",
                                    ModuleId = string.Format("viewmodels/maintenance.detail-templateid.{0}", t.MaintenanceTemplateId)
                                };
            var rdlRoutes = from t in rdls.ItemCollection
                                select new JsRoute
                                {
                                    Name = t.Title,
                                    Url = string.Format("reportdefinition.execute-id.{0}/:id", t.ReportDefinitionId),
                                    Caption = t.Title,
                                    Icon = "icon-bar-chart",
                                    ModuleId = string.Format("viewmodels/reportdefinition.execute-id.{0}", t.ReportDefinitionId)
                                };
         
            routes.AddRange(buildingRoute);
            routes.AddRange(csRoute);
            routes.AddRange(complaintFormRoutes);
            routes.AddRange(complaintAssignmentRoutes);
            routes.AddRange(complaintCloseRoutes);
            routes.AddRange(applicationRoutes);
            routes.AddRange(maintenanceRoute);
            routes.AddRange(rdlRoutes);
            return routes;
        } 
    }
}