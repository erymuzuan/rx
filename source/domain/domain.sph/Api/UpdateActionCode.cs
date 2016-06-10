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
            var arguments = pks.Select(m => m.GenerateParameterCode()).ToList();
            var parameters = pks.Select(m => m.Name.ToCamelCase());

            var version = !string.IsNullOrWhiteSpace(table.VersionColumn);
            var unmodifiedSince = !string.IsNullOrWhiteSpace(table.ModifiedDateColumn);
            if (version)
                arguments.Add("[IfMatch]ETag etag");

            if (unmodifiedSince)
                arguments.Add("[UnmodifiedSince]UnmodifiedSinceHeader unmodifiedSince");

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

            if (version)
            {
                code.Append(
       $@"               
                if (!etag.IsMatch(exist.{table.VersionColumn}.TimeStampToString()))
                {{
                    return Invalid((HttpStatusCode)428, new {{ message =""This request is required to be conditional;try using 'If-Match', or may be your resource is out of date""}});
                }}
                ");
            }
            if (unmodifiedSince)
            {
                code.Append(
       $@"               
                if (!unmodifiedSince.IsMatch(exist.{table.ModifiedDateColumn}))
                {{
                    return Invalid((HttpStatusCode)428, new {{ message =""This request is required to be conditional;try using 'If-Unmodified-Since', or may be your resource is out of date""}});
                }}
                ");
            }

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


        public override HypermediaLink GetHypermediaLink(Adapter adapter, TableDefinition table)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.MemberCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();
            return new HypermediaLink
            {
                Rel = "update",
                Method = "PUT",
                Href = $"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{parameters.ToString("/", x => $"{{{x}}}")}",
                Description = "Issue an UPDATE command"
            };
        }
    }
}