using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class GetOneActionCode : ControllerAction
    {
        public override bool Applicable(TableDefinition table)
        {
            return table.PrimaryKey != null;
        }

        public override string ActionName => "Get";
        public override string Name => "Get one by primary key";
        public CachingSetting CachingSetting { get; set; } = new CachingSetting
        {
            CacheControl = "Public",
            NoStore = true,
            Expires = 600
        };



        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name.ToCamelCase() + this.GetRouteConstraint(m) + "}");
            var arguments = pks.Select(m => m.GenerateParameterCode()).ToList();
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();

            var version = !string.IsNullOrWhiteSpace(table.VersionColumn);
            var modifiedDate = !string.IsNullOrWhiteSpace(table.ModifiedDateColumn);
            if(version || modifiedDate)
                arguments.Add($@" [SourceEntity(""{adapter.Id}"")]Bespoke.Sph.Domain.Api.Adapter adapterDefinition");
            if (version)
                arguments.Add("[IfNoneMatch]ETag etag");

            if (modifiedDate)
                arguments.Add("[ModifiedSince]ModifiedSinceHeader modifiedSince");


            var code = new StringBuilder();

            code.AppendLinf("       [Route(\"{0}\")]", string.Join("/", routeConstraint));
            code.AppendLinf("       [HttpGet]");
            code.AppendLinf("       public async Task<IHttpActionResult> Get({0})", arguments.ToString(","));
            code.AppendLine("       {");
            code.Append(
                $@"
            var context = new {table.ClrName}Adapter();
	        var result = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync({ErrorRetry.GenerateWaitCode()})
	                                .ExecuteAndCaptureAsync(async() => await context.LoadOneAsync({parameters.ToString(",\r\n\t\t\t\t")}) );

	        if(null != result.FinalException)
		        return InternalServerError(result.FinalException);

            var item = result.Result;

            if(null == item)
                return NotFound();
            
        ");

            var cachingCode = $@"
            var cacheSetting = adapterDefinition
                                .TableDefinitionCollection.Single(x => x.Name == ""{table.Name}"" && x.Schema == ""{table.Schema}"")
                                .ControllerActionCollection.OfType<{typeof(GetOneActionCode).FullName}>().Single()
                                .CachingSetting;";
            if (version || modifiedDate)
                code.AppendLine(cachingCode);

            if (version)
                code.AppendLine($"var version = item.{table.VersionColumn}.TimeStampToString();");


            if (version && modifiedDate)
                code.Append(
                    $@"
var cache = new CacheMetadata(version, item.{table.ModifiedDateColumn}, cacheSetting);
           ");
            if (version && !modifiedDate)
                code.Append(
                    @"
var cache = new CacheMetadata(version, null, cacheSetting);
           ");

            if (!version && modifiedDate)
                code.Append(
                    $@"
var cache = new CacheMetadata(null, item.{table.ModifiedDateColumn}, cacheSetting);
           ");


            if (modifiedDate)
                code.Append(
                    $@"
            if(modifiedSince.IsMatch(item.{table.ModifiedDateColumn}))
            {{
                return NotModified(cache);   
            }}");
            if (version)
                code.Append(
                    @"
            if(etag.IsMatch(version))
            {
                return NotModified(cache);   
            }");


            var links = table.ControllerActionCollection
                .Where(x => x.IsEnabled)
                .Where(x => x.Applicable(table))
                .Select(x => x.GetHypermediaLinks(adapter, table))
                .Where(x => null != x)
                .SelectMany(x => x)
                .Select(x => "{" + x.ToString().Replace("\"", "\"\"") + "}")
                .ToList();

            code.Append(
                $@"
            var json = JsonConvert.SerializeObject(item);
            var source = JObject.Parse(json);            
            var links = JArray.Parse($@""[{links.ToString(",\r\n")}]"");
            var link = new JProperty(""_links"", links);
            source.Last.AddAfterSelf(link);
            source.Remove(""WebId"");

            return Json(source.ToString(){((version || modifiedDate) ? ", cache" : "")});
");


            code.AppendLine();
            code.AppendLine("       }");

            return code.ToString();
        }

        public override HypermediaLink[] GetHypermediaLinks(Adapter adapter, TableDefinition table)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var parameters = pks.Select(m => m.Name.ToCamelCase() + (m.Type == typeof(DateTime) ? ":yyyy-MM-dd":"")).ToArray();
            return new[] {new HypermediaLink
            {
                Rel = "self",
                Method = "GET",
                Href = $"{{ConfigurationManager.BaseUrl}}/{adapter.RoutePrefix}/{table.Name.ToIdFormat()}/{parameters.ToString("/", x => $"{{{x}}}")}",
                Description = "Issue a GET request"
            }};
        }
    }
}