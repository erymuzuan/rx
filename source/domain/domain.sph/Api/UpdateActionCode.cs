using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof (ControllerAction))]
    public class UpdateActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();

            // update
            code.AppendLine("       [Route]");
            code.AppendLinf("       [HttpPut]");
            code.AppendLinf("       public async Task<HttpResponseMessage> Save([FromBody]{0} item)", table.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {0}Adapter();
            await context.UpdateAsync(item);

            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {{success = true, status=""OK"", item}} );
            return response;", table.Name);
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}