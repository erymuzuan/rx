using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class DeleteActionCode : ControllerAction
    {
        public override string Name => "Delete resource action";
        public override string ActionName => "Remove";

        public override bool Applicable(TableDefinition table)
        {
            return table.AllowDelete;
        }

        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name.ToCamelCase() + this.GetRouteConstraint(m) + "}");
            var methodParameters = pks.Select(m => m.GenerateParameterCode());
            var parameters = pks.Select(m => m.Name.ToCamelCase());
            var parameterArguments = string.Join(",", parameters);

            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint.ToArray()));
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<IHttpActionResult> Remove({0})", string.Join(",", methodParameters));
            code.AppendLine("       {");
            code.Append(
                $@"
            var context = new {table.ClrName}Adapter();
            var item = await context.LoadOneAsync({parameterArguments});
            if(null == item)
                return NotFound();
            await context.DeleteAsync({parameterArguments});

            return Ok();");
            code.AppendLine();
            code.AppendLine("       }");
            return code.ToString();
        }

        public override HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            if (!this.Applicable(table)) return null;
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();
            return new[] {new HypermediaLink
            {
                Rel = "delete",
                Method = "DELETE",
                Href = $"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{parameters.ToString("/", x => $"{{{x}}}")}",
                Description = "Issue a DELETE command to your table"
            }};

        }
    }
}