using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ReportDefinitionController : Controller
    {
        public async Task<ActionResult> Index(int id = 0)
        {
            var context = new SphDataContext();
            var rdl = await context.LoadOneAsync<ReportDefinition>(x => x.ReportDefinitionId == id) ?? new ReportDefinition();

            var items = new ObjectCollection<ReportItem>
            {
                new LabelItem{Name = "Label",CssClass = "",Icon = "icon-font"},
                new DataGridItem{Name = "Table",Icon = "icon-table"},
                new LineChartItem{Name = "Line chart",Icon = "icon-bar-chart"},
                new LineItem{Name = "Horizontal line",Icon = "icon-ellipsis-horizontal"},
                new PieChartItem{Name = "Pie chart",Icon = "icon-circle"},
                new BarChartItem{Name = "Bar chart",Icon = "icon-bar-chart"}
            };

            var vm = new ReportBuilderViewModel
            {
                ReportDefinition = rdl,

            };
            vm.ReportItems.AddRange(items);
            vm.ToolboxItems.AddRange(items);

            return View(vm);
        }

        public async Task<ActionResult> Save()
        {
            var rdl = this.GetRequestJson<ReportDefinition>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(rdl);
                await session.SubmitChanges("Save RDL");
            }


            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(rdl.ReportDefinitionId));


        }

        public async Task<ActionResult> GetEntityColumns(string entityName)
        {
            var dataSource = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = (typeof (Entity).AssemblyQualifiedName ?? "").Replace("Entity", entityName);
            var cols = await dataSource.GetColumnsAsync(Type.GetType(typeName));
            return Json(cols, JsonRequestBehavior.AllowGet);
        }
    }
}
