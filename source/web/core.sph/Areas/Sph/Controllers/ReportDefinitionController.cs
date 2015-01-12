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

        public async Task<ActionResult> Remove()
        {
            var rdl = this.GetRequestJson<ReportDefinition>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Delete(rdl);
                await session.SubmitChanges("Delete");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new { success = true, id = rdl.Id });


        }
        public async Task<ActionResult> Save()
        {
            var rdl = this.GetRequestJson<ReportDefinition>();
            var context = new SphDataContext();

            // update the Filter type based on the FieldName
            var dataSource = ObjectBuilder.GetObject<IReportDataSource>();
            var typeName = rdl.DataSource.EntityName;
            var cols = await dataSource.GetColumnsAsync(typeName);

            var vaild = await rdl.ValidateBuildAsync();
            if (!vaild.Result)
                return Json(vaild);

            if (rdl.IsNewItem)
                rdl.Id = (rdl.DataSource.EntityName + "-" + rdl.Title).ToIdFormat();

            foreach (var filter in rdl.DataSource.ReportFilterCollection)
            {
                var filter1 = filter;
                filter.TypeName = cols.Single(c => c.Name == filter1.FieldName).TypeName;
            }


            using (var session = context.OpenSession())
            {
                session.Attach(rdl);
                await session.SubmitChanges("Save");
            }

            return Json(new { success = true, status = "OK", id = rdl.Id, message = "Your RDL has been successfuly saved" });


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
