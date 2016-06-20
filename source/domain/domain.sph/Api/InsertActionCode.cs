using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class InsertActionCode : ControllerAction
    {
        public override string Name => "Insert new resource action";
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            var code = new StringBuilder();
            var url = $@"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{{item.{table.PrimaryKey?.Name}}}";
            if (null == table.PrimaryKey)
                url = $@"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}";

            // insert
            code.AppendLine("       [Route(\"\")]");
            code.AppendLinf("       [HttpPost]");
            code.AppendLinf("       public async Task<IHttpActionResult> Insert([FromBody]{0} item)", table.Name);
            code.AppendLine("       {");
            code.Append(
                $@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new {table.Name}Adapter();
            var result = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(500 * c))
	                                .ExecuteAndCaptureAsync(async() => await context.InsertAsync(item));

	        if(null != result.FinalException)
		        return InternalServerError(result.FinalException);
            
            return Created(new Uri($""{url}""), item);
            ");
            code.AppendLine();

            code.AppendLine("       }");

            return code.ToString();
        }
    }
}