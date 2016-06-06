using System.ComponentModel.Composition;
using System.Text;
using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class UpdateActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;

            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name.ToCamelCase() + this.GetRouteConstraint(m) + "}");
            var arguments = pks.Select(m => m.GenerateParameterCode());
            var parameters = pks.Select(m => m.Name.ToCamelCase());


            var code = new StringBuilder();

            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint));

            // update
            code.AppendLinf("       [HttpPut]");
            code.AppendLine($"       public async Task<IHttpActionResult> Save([FromBody]{table.Name} item, {arguments.ToString(",")})");
            code.AppendLine("       {");
            code.AppendLine(
                $@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {table
                    .Name}Adapter();
            var exist = await context.LoadOneAsync({string.Join(",", parameters)});

            if(null == exist)
                return NotFound();");

            foreach (var pk in table.PrimaryKeyCollection)
            {
                code.AppendLine($"item.{pk} = {pk.ToCamelCase()};");
            }

            code.AppendLine($@"

            await context.UpdateAsync(item);

            return Ok();");
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}