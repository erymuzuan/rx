using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class ReportDefinitionController : Controller
    {

        public async Task<ActionResult> Save()
        {
            var rdl = this.GetRequestJson<ReportDefinition>();
            var context = new SphDataContext();

            // update the Filter type based on the FieldName
            var dataSource = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = rdl.DataSource.EntityName;
            var cols = await dataSource.GetColumnsAsync(typeName);

            foreach (var filter in rdl.DataSource.ReportFilterCollection)
            {
                var filter1 = filter;
                filter.TypeName = cols.Single(c => c.Name == filter1.FieldName).TypeName;
            }


            using (var session = context.OpenSession())
            {
                session.Attach(rdl);
                await session.SubmitChanges("Save RDL");
            }


            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(rdl.ReportDefinitionId));


        }


        public async Task<ActionResult> GetEntityColumns(string id)
        {
            var dataSource = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = id;
            var cols = await dataSource.GetColumnsAsync(typeName);

            var json = JsonConvert.SerializeObject(cols);

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(json);
        }
    }
}
