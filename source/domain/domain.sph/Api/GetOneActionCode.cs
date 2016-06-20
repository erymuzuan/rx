using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    [Export(typeof(ControllerAction))]
    public class GetOneActionCode : ControllerAction
    {
        public override string ActionName => "Get";
        public override string Name => "Get one by primary key";

        [ImportMany(typeof(ControllerAction))]
        [JsonIgnore]
        public ControllerAction[] ActionCodeGenerators { get; set; }

        public override string GenerateCode(TableDefinition table, Adapter adapter)
        {
            if (table.PrimaryKeyCollection.Count == 0) return null;
            var pks = table.ColumnCollection.Where(m => table.PrimaryKeyCollection.Contains(m.Name)).ToArray();
            var routeConstraint = pks.Select(m => "{" + m.Name.ToCamelCase() + this.GetRouteConstraint(m) + "}");
            var arguments = pks.Select(m => m.GenerateParameterCode()).ToList();
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();

            var version = !string.IsNullOrWhiteSpace(table.VersionColumn);
            var modifiedDate = !string.IsNullOrWhiteSpace(table.ModifiedDateColumn);
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
            var context = new {table.Name}Adapter();
	        var result = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(500 * c))
	                                .ExecuteAndCaptureAsync(async() => await context.LoadOneAsync({parameters.ToString(",")}) );

	        if(null != result.FinalException)
		        return InternalServerError(result.FinalException);

            var item = result.Result;

            if(null == item)
                return NotFound();
            
        ");

            if (version)
                code.AppendLine($"var version = item.{table.VersionColumn}.TimeStampToString();");


            if (version && modifiedDate)
                code.Append(
                    $@"
var cache = new CacheMetadata(version, item.{table.ModifiedDateColumn});
           ");
            if (version && !modifiedDate)
                code.Append(
                    @"
var cache = new CacheMetadata(version, null);
           ");

            if (!version && modifiedDate)
                code.Append(
                    $@"
var cache = new CacheMetadata(null, item.{table.ModifiedDateColumn});
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

            if (null == this.ActionCodeGenerators)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.ActionCodeGenerators) throw new Exception($"Fail to initialize {nameof(GetOneActionCode)}");

            var links = this.ActionCodeGenerators.Select(x => x.GetHypermediaLinks(adapter, table))
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
            var parameters = pks.Select(m => m.Name.ToCamelCase()).ToArray();
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