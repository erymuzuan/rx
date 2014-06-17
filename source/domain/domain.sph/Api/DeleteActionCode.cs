using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof (ControllerAction))]
    public class DeleteActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name + this.GetRouteConstraint(m.Type) + "}");
            var arguments = pks.Select(m => m.Type.ToCSharp() + " " + m.Name);
            var parameters = pks.Select(m => m.Name);
            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint.ToArray()));
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove({0})", string.Join(",", arguments));
            code.AppendLine("       {");
            code.AppendFormat(@"
            var context = new {0}Adapter();
            await context.DeleteAsync({1});
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 202;
            return Json(new {{success = true, status=""OK""}});", table.Name, string.Join(",", parameters));
            code.AppendLine();
            code.AppendLine("       }");
            return code.ToString();
        }
    }
}