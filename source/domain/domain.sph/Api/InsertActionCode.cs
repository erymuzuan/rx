using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof (ControllerAction))]
    public class InsertActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();
            // insert
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Insert([RequestBody]{0} item)", table.Name);
            code.AppendLine("       {");
            code.AppendFormat(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {0}Adapter();
            await context.InsertAsync(item);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK"", item}});", table.Name);
            code.AppendLine();
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}