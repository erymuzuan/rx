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
            code.AppendLine("       [Route(\"\")]");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<HttpResponseMessage> Insert([FromBody]{0} item)", table.Name);
            code.AppendLine("       {");
            code.AppendFormat(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {0}Adapter();
            await context.InsertAsync(item);
            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {{success = true, status=""OK"", item}} );
            return response;", table.Name);
            code.AppendLine();
            code.AppendLine("       }");

            return code.ToString();
        }
    }
}