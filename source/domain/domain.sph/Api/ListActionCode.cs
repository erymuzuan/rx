using System;
using System.Collections.Generic;
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
        public override string[] GetActionNames(TableDefinition table, Adapter adapter)
        {
            return new[] {"ListAsync"};
        }

        public override bool IsAsync => true;
        public override Type ReturnType => typeof(Task<IHttpActionResult>);
        public override string Route => "";
        public CachingSetting CachingSetting { get; set; }   = new CachingSetting
        {
            CacheControl = "Public",
            NoStore = true,
            Expires = 600
        };

        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            //if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name))
                .Select(x => $"{{item.{x.Name}}}")
                .ToString("/");
            var objectLinks = "";
            if (!string.IsNullOrWhiteSpace(pks))
                objectLinks = $@"        
                JArray links = JArray.Parse($@""[
		                                {{{{
	                                """"method"""": """"GET"""",
	                                """"rel"""": """"self"""",
	                                """"href"""": """"{{ConfigurationManager.BaseUrl}}/api/{adapter.Id}/{table.Name.ToIdFormat()}/{pks}"""",
	                                """"desc"""": """"Issue a GET request""""
	                                }}}}
		                                ]"");
		        var link = new JProperty(""_links"", links);
		        r.Last.AddAfterSelf(link);";


            var arguments = new List<string>();

            var modifiedDate = !string.IsNullOrWhiteSpace(table.ModifiedDateColumn);
            if (modifiedDate)
            {
                arguments.Add($@" [SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition");
                arguments.Add("[ModifiedSince]ModifiedSinceHeader modifiedSince");
            }

      
            var cachingCode = $@"
            var cacheSetting = adapterDefinition
                                .TableDefinitionCollection.Single(x => x.Name == ""{table.Name}"" && x.Schema == ""{table.Schema}"")
                                .ControllerActionCollection.OfType<{typeof(ListActionCode).FullName}>().Single()
                                .CachingSetting;
            var maxTranslator = new {adapter.OdataTranslator}<{table.ClrName}>(""{table.ModifiedDateColumn}"",""{table.Name}""){{Schema = ""{table.Schema}""}};
            var getMaxModifiedDateSql = maxTranslator.Max(filter);
            var modifiedDate = (await context.ExecuteScalarAsync<DateTime?>(getMaxModifiedDateSql)) ?? System.DateTime.Now;
            var cache = new CacheMetadata(null, modifiedDate, cacheSetting);
            
            if(modifiedSince.IsMatch(modifiedDate))
            {{
                return NotModified(cache);   
            }}";

            arguments.Add("string filter = null");
            arguments.Add("int page = 1");
            arguments.Add("int size = 40");
            arguments.Add("bool includeTotal = false");
            arguments.Add("string orderby = null");

            var code = new StringBuilder();
            code.AppendLine("       [Route(\"\")]");
            code.AppendLine("       [HttpGet]");
            code.AppendLine($@"       
                                    public async Task<IHttpActionResult> ListAsync({arguments.ToString(",\r\n\t\t\t\t")})");
            code.AppendLine("       {");
            code.Append($@"
           if (size > 200)
                throw new ArgumentException(""Your are not allowed to do more than 200"", ""size"");

            var translator = new {adapter.OdataTranslator}<{table.ClrName}>(null,""{table.Name}"" ){{Schema = ""{table.Schema}""}};
            var sql = translator.Select(filter, orderby);
            var count = 0;

            var context = new {table.ClrName}Adapter();
            var nextPageToken = string.Empty;
            {(modifiedDate ? cachingCode : "")}
            
            var loResult = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync({ErrorRetry.GenerateWaitCode()})
	                                .ExecuteAndCaptureAsync(async() => await context.LoadAsync(sql, page, size));

	        if(null != loResult.FinalException)
		        return InternalServerError(loResult.FinalException);
            
            var lo = loResult.Result;


            if (includeTotal || page > 1)
            {{
                var translator2 = new {adapter.OdataTranslator}<{table.ClrName}>(null, ""{table.Name}""){{Schema = ""{table.Schema}""}};
                var countSql = translator2.Count(filter);

                var countResult = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(500 * c))
	                                .ExecuteAndCaptureAsync(async() => await context.ExecuteScalarAsync<int>(countSql));

	            if(null != countResult.FinalException)
		            throw countResult.FinalException;
                count = countResult.Result;
                if (page * size < count)
                    nextPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page + 1}}&size={{size}}"";
                else
                    nextPageToken = null;
            }}

            var previousPageToken = $""{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/?filter={{filter}}&includeTotal=true&page={{page - 1}}&size={{size}}"";
            if(page == 1)
                previousPageToken = null;

            var pageLinks = new System.Collections.Generic.List<object>();
            if(null != previousPageToken)
                pageLinks.Add(new {{ method = ""GET"", rel= ""previous"", href= previousPageToken}});
            if(null != nextPageToken)
                pageLinks.Add(new {{ method = ""GET"", rel= ""next"", href= nextPageToken}});


            var json = JObject.Parse( JsonConvert.SerializeObject( new
            {{
                count,
                page,
                size,
                _links = pageLinks
            }}));

            JArray results = JArray.Parse(""[]"");
	        foreach (var item in lo.ItemCollection)
	        {{
		        var r = JObject.Parse(JsonConvert.SerializeObject(item));
		        r.Remove(""WebId"");
		        {objectLinks}

		        results.Add(r);
	        }}
	        json.Last.AddBeforeSelf(new JProperty(""results"", results));

            return Json(json.ToString(){(( modifiedDate) ? ", cache" : "")});
            ");



            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();

            return code.ToString();
        }


    }
}