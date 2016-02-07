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
            code.AppendLine("public async Task<IHttpActionResult> GetOneByIdAsync(string id, [IfNoneMatch]ETag etag, [ModifiedSince]DateTimeOffset? modifiedSince)");
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
            if(null == lo.Source) return NotFound(""Cannot find {ed.Name} with Id "" + id);            

            var cacheSetting = setting.ResourceEndpointSetting.CachingSetting;
            var cache = new CacheControlHeaderValue();
/*
            if(cacheSetting.Expires.HasValue)
                cetExpires(DateTime.UtcNow.AddSeconds(cacheSetting.Expires.Value));
            if(cacheSetting.NoStore)
                this.Response.Cache.SetNoStore();
            if(!string.IsNullOrWhiteSpace(cacheSetting.CacheControl))
                this.Response.Cache.SetCacheability((HttpCacheability)Enum.Parse(typeof(HttpCacheability), cacheSetting.CacheControl));


            this.Response.Cache.SetETag(lo.Version);
            this.Response.AppendHeader(""ETag"", lo.Version);
            this.Response.Cache.SetLastModified(lo.Source.ChangedDate);
*/           
            if($""{{modifiedSince}}"" == lo.Source.ChangedDate.ToString())
            {{
                return NotModified();                      
            }}           
                
            if (etag.Tag == lo.Version)
            {{
                return NotModified();   
            }}
                
            var source = JObject.Parse(lo.Json ?? lo.Source.ToJson());    
            var links = JArray.Parse($@""[{operations}]""); 

            var link = new JProperty(""_links"", links);
            source.Last.AddAfterSelf(link);

            return Json(source.ToString());
            ");


            code.AppendLine();
            code.AppendLine("}");
            return new Method { Code = code.ToString() };


        }
    }
}