using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
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
                Console.WriteLine("ROWS ------------- " + rows.Count);
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
