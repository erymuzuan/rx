using System.ComponentModel.Composition;
using System.Text;
using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class InsertActionCode : ControllerAction
    {
        public override bool Applicable(TableDefinition table)
        {
            if (!table.AllowInsert)
                return false;
            var unsupportedNonNull = table.ColumnCollection.Any(x => x.Unsupported && !x.IsNullable);
            return !unsupportedNonNull;
        }

        public override string[] GetActionNames(TableDefinition table, Adapter adapter)
        {
            return new[] { "InsertAsync" };
        }

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
            code.AppendLine($@"       public async Task<IHttpActionResult> InsertAsync(
                                                                [SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition,
                                                                [FromBody]{table.ClrName} item)");
            code.AppendLine("       {");
            code.AppendLine(@"          if(null == item) throw new ArgumentNullException(""item"");");
            code.AppendLine("       ");

            // generate default value for NOT NULL and it's Ignored || IsComplex
            code.AppendLine("var rc = new RuleContext(item);");
            var defaultValueCodes = table.ColumnCollection.Select(x => x.GetDefaultValueCode(table));
            code.JoinAndAppendLine(defaultValueCodes, "\r\n");

            code.Append(
                $@"


            var context = new {table.ClrName}Adapter();
            var result = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync({ErrorRetry.GenerateWaitCode()})
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