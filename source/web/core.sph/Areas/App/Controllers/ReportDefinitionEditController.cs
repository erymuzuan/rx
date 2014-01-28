using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class ReportDefinitionEditController : BaseAppController
    {
        public ActionResult Html()
        {
            var items = new ObjectCollection<ReportItem>
            {
                new LabelItem{Name = "Label",CssClass = "",Icon = "icon-font"},
                new DataGridItem{Name = "Table",Icon = "icon-table"},
                new LineChartItem{Name = "Line chart",Icon = "icon-bar-chart"},
                new LineItem{Name = "Horizontal line",Icon = "icon-ellipsis-horizontal"},
                new PieChartItem{Name = "Pie chart",Icon = "icon-circle"},
                new BarChartItem{Name = "Bar chart",Icon = "icon-bar-chart"}
            };

            var vm = new ReportBuilderViewModel();
            vm.ReportItems.AddRange(items);
            vm.ToolboxItems.AddRange(items);
            return View(vm);
        }
        public async Task<ActionResult> ReportDefinitionExecuteJs(int id)
        {
            var context = new SphDataContext();
            var rdl = await context.LoadOneAsync<ReportDefinition>(r => r.ReportDefinitionId == id);

            var view = this.RenderRazorViewToJs("ReportDefinitionExecuteJs", rdl);
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            return Content(view);
        }

        public async Task<ActionResult> ReportDefinitionExecuteHtml(int id)
        {
            var context = new SphDataContext();
            var datasource = this.GetRequestJson<DataSource>();
            ReportDefinition rdl = null;
            rdl = await context.LoadOneAsync<ReportDefinition>(r => r.ReportDefinitionId == id);
            if (null != datasource)
            {
                await Task.Delay(500);
                rdl.DataSource = datasource;
                var rows = await rdl.ExecuteResultAsync();

                rdl.ReportLayoutCollection.SelectMany(l => l.ReportItemCollection)
                    .ToList()
                    .ForEach(t =>
                    {
                        t.SetRows(rows);
                        t.SetRdl(rdl);
                    });
            }



            var vm = new RdlExecutionViewModel
            {
                Rdl = rdl,
                IsPostback = null != datasource
            };

            return View(vm);
        }


        public async Task<ActionResult> ReportDefinitionPreview()
        {
            var rdl = this.GetRequestJson<ReportDefinition>();
            var datasource = rdl.DataSource;

            var sql = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = rdl.DataSource.EntityName;
            var cols = await sql.GetColumnsAsync(typeName);

            foreach (var filter in rdl.DataSource.ReportFilterCollection)
            {
                var filter1 = filter;
                filter.TypeName = cols.Single(c => c.Name == filter1.FieldName).TypeName;
            }

            if (null != datasource)
            {
                await Task.Delay(500);
                rdl.DataSource = datasource;
                var rows = await rdl.ExecuteResultAsync();

                rdl.ReportLayoutCollection.SelectMany(l => l.ReportItemCollection)
                    .ToList()
                    .ForEach(t =>
                    {
                        t.SetRows(rows);
                        t.SetRdl(rdl);
                    });
            }



            var vm = new RdlExecutionViewModel
            {
                Rdl = rdl,
                IsPostback = null != datasource
            };

            return View(vm);

        }
    }
}