using System.Collections.Generic;
using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class EntityResourceEndpoint : DomainObject
    {
        public Method GenerateGetByIdCode(EntityDefinition ed)
        {
            var context = new SphDataContext();
            var code = new StringBuilder();
            code.AppendLine("[HttpGet]");
            code.AppendLine("[Route(\"{id:guid}\")]");
            code.AppendLine("public async Task<ActionResult> GetOneByIdAsync(string id)");
            code.AppendLine("{");

            var links = new List<string>
            {
                $@"{{{{
                    """"rel"""" : """"self"""", 
                    """"href"""" : """"{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{id}}"""" 
                }}}}"
            };
            var operationEndpoints = context.LoadFromSources<OperationEndpoint>(x => x.Entity == ed.Name);
            foreach (var op in operationEndpoints)
            {
                var methods = new List<string>();
                if (op.IsHttpDelete) methods.Add("DELETE");
                if (op.IsHttpPatch) methods.Add("PATCH");
                if (op.IsHttpPost) methods.Add("POST");
                if (op.IsHttpPut) methods.Add("PUT");
                var http = string.Join("|", methods);
                if (string.IsNullOrWhiteSpace(http)) continue;

                var route = op.Route;
                route = route.StartsWith("~/") ? route.Replace("~/", "") : $"api/{op.Resource}/{route}";

                links.Add($@"  {{{{
                    """"rel"""" : """"{op.Name}"""", 
                    """"href"""" : """"{{ConfigurationManager.BaseUrl}}/{route}"""", 
                    """"method"""" : """"{http}"""" ,
                    """"desc"""" : """"{op.Note}"""", 
                    """"doc"""" : """"{{ConfigurationManager.BaseUrl}}/api/docs/{ed.Id}#{op.Name}"""" 
                }}}}");
            }
            var operations = string.Join(",", links);
            code.Append($@"");
            code.Append($@"
            var url = $""{ConfigurationManager.ApplicationName.ToLower()}/{ed.Name.ToLower()}/{{id}}"";
            var ed = CacheManager.Get<EntityDefinition>(""{ed.Id}"");
            if(null == ed)
            {{
                ed = EntityDefinitionSource.DeserializeFromJsonFile<EntityDefinition>();
                CacheManager.Insert(""{ed.Id}"", ed, EntityDefinitionSource);
            }}
            var setting = await ed.ServiceContract.LoadSettingAsync(ed.Name);
            
            var esResponseString = string.Empty;
            using(var client = new HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.GetAsync(url);
                if(response.StatusCode == HttpStatusCode.NotFound)
                    return HttpNotFound(""Cannot find any {ed.Name} with Id "" + id);

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" );
                esResponseString = await content.ReadAsStringAsync();
            }}
            var esJson = JObject.Parse(esResponseString);
            var version = esJson.SelectToken(""$._version"").Value<string>();
            var changedDate = esJson.SelectToken(""$._source.ChangedDate"").Value<DateTime>();

            var cacheSetting = setting.ResourceEndpointSetting.CachingSetting;
            if(cacheSetting.Expires.HasValue)
                this.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(cacheSetting.Expires.Value));
            if(cacheSetting.NoStore)
                this.Response.Cache.SetNoStore();
            if(!string.IsNullOrWhiteSpace(cacheSetting.CacheControl))
                this.Response.Cache.SetCacheability((HttpCacheability)Enum.Parse(typeof(HttpCacheability), cacheSetting.CacheControl));


            this.Response.Cache.SetETag(version);
            this.Response.AppendHeader(""ETag"", version);
            this.Response.Cache.SetLastModified(changedDate);

            DateTime modifiedSince;
            if (DateTime.TryParse(this.Request.Headers[""If-Modified-Since""], out modifiedSince))
            {{
                if(modifiedSince.ToString() == changedDate.ToString())
                {{
                    this.Response.StatusCode = 304;
                    return Content(string.Empty,""application/json; charset=utf-8"");                        
                }}
            }}
                
            if (this.Request.Headers[""If-None-Match""] == version)
            {{
                this.Response.StatusCode = 304;
                return Content(string.Empty,""application/json; charset=utf-8"");
            }}
                
                
            var source = esJson.SelectToken(""$._source"");
            var links = JArray.Parse($@""[{operations}]""); 

            var link = new JProperty(""_links"", links);
            source.Last.AddAfterSelf(link);

                return Content(source.ToString(), ""application/json; charset=utf-8"");
            ");


            code.AppendLine();
            code.AppendLine("}");
            return new Method { Code = code.ToString() };


        }
    }
}