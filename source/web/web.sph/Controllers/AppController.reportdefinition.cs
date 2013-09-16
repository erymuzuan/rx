using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult ReportDefinitionEditHtml()
        {
            return RedirectToAction("Index", "ReportDefinition");
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
            var typeName = (typeof(Entity).GetShortAssemblyQualifiedName() ?? "").Replace("Entity", rdl.DataSource.EntityName);
            var cols = await sql.GetColumnsAsync(Type.GetType(typeName));

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

    public class RdlExecutionViewModel
    {
        public ReportDefinition Rdl { get; set; }
        public bool IsPostback { get; set; }
    }
}
