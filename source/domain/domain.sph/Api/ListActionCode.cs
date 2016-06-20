using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class ListActionCode : ControllerAction
    {
        public override string Name => "List action";
        public ErrorRetry ErrorRetry { get; set; } = new ErrorRetry { Wait = 500, Algorithm = WaitAlgorithm.Linear, Attempt = 3 };
        public override string ActionName => "List";
        public override bool IsAsync => true;
        public override Type ReturnType => typeof(Task<IHttpActionResult>);
        public override string Route => "";

        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name))
                .Select(x => $"{{item.{x.Name}}}")
                .ToString("/");


            var code = new StringBuilder();
            code.AppendLine("       [Route(\"\")]");
            code.AppendLine("       [HttpGet]");
            code.AppendLine($@"       
                                    public async Task<IHttpActionResult> {ActionName}(string filter = null, 
                                                    int page = 1, 
                                                    int size = 40, 
                                                    bool includeTotal = false, 
                                                    string orderby = null)");
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
	                                .WaitAndRetryAsync({ErrorRetry.GenerateWaitCode()})
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

        public override string GetDesignerHtmlView()
        {
            return $@"       
                <form data-bind=""with:ErrorRetry"">
                    <div class=""form-group"">
                        <label for=""retry-count"" class=""control-label"">Attempt</label>
                        <input type=""number"" max=""50"" data-bind=""value: Attempt, tooltip:'Enable retry for your database call, the min value is 2, in an Exception is thrown, after the number of retry count you set, the execution will stop and exception is propagated to the call stack'"" min=""2""
                               placeholder=""Set the number if retries if the invocation throws any exception""
                               class=""form-control"" id=""retry-count"">
                    </div>
                    <div class=""form-group"">
                        <label for=""retry-interval"" class=""control-label"">Wait</label>
                        <input type=""number"" step=""10"" max=""50000"" data-bind=""value: Wait, tooltip:'The time in ms, the code will wait before attempting the next retry. The default is 500ms'""
                               placeholder=""The interval between retries in ms""
                               class=""form-control"" id=""retry-interval"">
                    </div>
                    <div class=""form-group"">
                        <label for=""retry-interval"" class=""control-label"">Algorithm</label>
                        <select data-bind=""value: Algorithm, tooltip:'Connstant - set to your interval value, Liner = interval * n, Exponential = interval * (2^n), n is the retry attempt'""
                                class=""form-control"" id=""retry-wait"">
                            <option value=""Constant"">Constant</option>
                            <option value=""Linear"">Linear</option>
                            <option value=""Exponential"">Exponential</option>
                        </select>
                    </div>
                </form>";
        }
    }
}