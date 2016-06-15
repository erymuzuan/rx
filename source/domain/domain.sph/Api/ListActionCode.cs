using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ListActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {

            var code = new StringBuilder();
            code.AppendLine("       [Route(\"\")]");
            code.AppendLine("       [HttpGet]");
            code.AppendLinf(
                "       public async Task<IHttpActionResult> List(string filter = null, int page = 1, int size = 40, bool includeTotal = false, string orderby = null)");
            code.AppendLine("       {");
            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var translator = new {adapter.OdataTranslator}<{table.Name}>(null,""{table.Name}"" ){{Schema = ""{table.Schema}""}};
            var sql = translator.Select(filter, orderby);
            var count = 0;

            var context = new {table.Name}Adapter();
            var nextPageToken = string.Empty;
            
            var loResult = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(500 * c))
	                                .ExecuteAndCaptureAsync(async() => await context.LoadAsync(sql, page, size));

	        if(null != loResult.FinalException)
		        throw loResult.FinalException;
            var lo = loResult.Result;


            if (includeTotal || page > 1)
            {{
                var translator2 = new {adapter.OdataTranslator}<{table.Name}>(null, ""{table.Name}""){{Schema = ""{table.Schema}""}};
                var countSql = translator2.Count(filter);

                var countResult = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(500 * c))
	                                .ExecuteAndCaptureAsync(async() => await context.ExecuteScalarAsync<int>(countSql));

	            if(null != countResult.FinalException)
		            throw countResult.FinalException;
                count = countResult.Result;
                if (count > lo.ItemCollection.Count())
                    nextPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page + 1}}&size={{size}}"";
                else
                    nextPageToken = null;
            }}

            var previousPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page - 1}}&size={{size}}"";
            if(page == 1)
                previousPageToken = null;
            var json = new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size,
                results = lo.ItemCollection.ToArray()
            }};
            return Ok(json);
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}