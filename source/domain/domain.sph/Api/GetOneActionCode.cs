using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class GetOneActionCode : ControllerAction
    {
        public override string ActionName => "Get";

        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name.ToCamelCase() + this.GetRouteConstraint(m) + "}");
            var arguments = pks.Select(m => m.GenerateParameterCode());
            var parameters = pks.Select(m => m.Name.ToCamelCase());


            var code = new StringBuilder();

            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint));
            code.AppendLinf("       [HttpGet]");
            code.AppendLinf("       public async Task<IHttpActionResult> Get({0})", string.Join(",", arguments));
            code.AppendLine("       {");
            code.AppendLinf(@"
            var context = new {0}Adapter();
            var item = await context.LoadOneAsync({1});

            if(null == item)
                return NotFound();
            var json = JsonConvert.SerializeObject(item);
            // TODO : insert a __links proeprty for hypermedia
            // var jo = JObject.Parse(json);
            
            return Json(json);
", table.Name, string.Join(",", parameters));
            code.AppendLine();
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}