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
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);  
            if(null == lo.Source) return HttpNotFound(""Cannot find {ed.Name} with Id "" + id);            

            var cacheSetting = setting.ResourceEndpointSetting.CachingSetting;
            if(cacheSetting.Expires.HasValue)
                this.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(cacheSetting.Expires.Value));
            if(cacheSetting.NoStore)
                this.Response.Cache.SetNoStore();
            if(!string.IsNullOrWhiteSpace(cacheSetting.CacheControl))
                this.Response.Cache.SetCacheability((HttpCacheability)Enum.Parse(typeof(HttpCacheability), cacheSetting.CacheControl));


            this.Response.Cache.SetETag(lo.Version);
            this.Response.AppendHeader(""ETag"", lo.Version);
            this.Response.Cache.SetLastModified(lo.Source.ChangedDate);

            DateTime modifiedSince;
            if (DateTime.TryParse(this.Request.Headers[""If-Modified-Since""], out modifiedSince))
            {{
                if(modifiedSince.ToString() == lo.Source.ChangedDate.ToString())
                {{
                    this.Response.StatusCode = 304;
                    return Content(string.Empty,""application/json; charset=utf-8"");                        
                }}
            }}
                
            if (this.Request.Headers[""If-None-Match""] == lo.Version)
            {{
                this.Response.StatusCode = 304;
                return Content(string.Empty,""application/json; charset=utf-8"");
            }}
                
            var source = JObject.Parse(lo.Json ?? lo.Source.ToJson());    
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