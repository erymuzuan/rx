using System.ComponentModel.Composition;
using System.Text;
using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ListActionCode : ControllerAction
    {
        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name))
                .Select(x => $"{{item.{x.Name}}}")
                .ToString("/");

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
		        return InternalServerError(loResult.FinalException);
            
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
            var json = JObject.Parse( JsonConvert.SerializeObject( new
            {{
                count,
                page,
                nextPageToken,
                previousPageToken,
                size
            }}));

            JArray results = JArray.Parse(""[]"");
	        foreach (var item in lo.ItemCollection)
	        {{
		        var r = JObject.Parse(JsonConvert.SerializeObject(item));
		        r.Remove(""WebId"");
		        JArray links = JArray.Parse($@""[
		                                {{{{
	                                """"method"""": """"GET"""",
	                                """"rel"""": """"self"""",
	                                """"href"""": """"{{ConfigurationManager.BaseUrl}}/api/{adapter.Id}/{table.Name.ToIdFormat()}/{pks}"""",
	                                """"desc"""": """"Issue a GET request""""
	                                }}}}
		                                ]"");
		        var link = new JProperty(""_links"", links);
		        r.Last.AddAfterSelf(link);

		        results.Add(r);
	        }}

	        json.Last.AddAfterSelf(new JProperty(""results"", results));

            return Json(json.ToString());
            ");


            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }
    }
}