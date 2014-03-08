using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class ReportDefinitionExecuteController : BaseAppController
    {
        [RazorScriptFilter]
        public async Task<ActionResult> Js(int id)
        {
            var context = new SphDataContext();
            var rdl = await context.LoadOneAsync<ReportDefinition>(r => r.ReportDefinitionId == id);

            return View("Script", rdl);
        }

        public async Task<ActionResult> Html(int id)
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


        public async Task<ActionResult> Preview()
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