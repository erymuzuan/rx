using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class GetOneActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name + this.GetRouteConstraint(m.Type) + "}");
            var arguments = pks.Select(m => m.Type.ToCSharp() + " " + m.Name);
            var parameters = pks.Select(m => m.Name);


            var code = new StringBuilder();

            code.AppendLinf("       [Route(\"{{{0}}}\")]", string.Join("/", routeConstraint));
            code.AppendLinf("       [HttpGet]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Get({0})", string.Join(",", arguments));
            code.AppendLine("       {");
            code.AppendLinf(@"
            var context = new {0}Adapter();
            var item  =await context.LoadOneAsync({1});
            this.Response.ContentType = ""application/json"";
            this.Response.StatusCode = 200;
            if(null == item)
                return Content(JsonConvert.SerializeObject(new {{success = false, status = ""NotFound"", statusCode=404, url=""/api/docs/404"", message =""item not found""}}));
            return Content(JsonConvert.SerializeObject(new {{success = true, status = ""OK"", item}}));
", table.Name, string.Join(",", parameters));
            code.AppendLine();
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}