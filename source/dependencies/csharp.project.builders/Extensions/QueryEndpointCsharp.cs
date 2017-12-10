using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public class QueryEndpointCsharp
    {
        public QueryEndpoint Endpoint { get; }
        public EntityDefinition EntityDefinition { get; }

        public QueryEndpointCsharp(QueryEndpoint endpoint, EntityDefinition entityDefinition)
        {
            Endpoint = endpoint;
            EntityDefinition = entityDefinition;
        }
        public QueryEndpointCsharp(QueryEndpoint endpoint)
        {
            Endpoint = endpoint;
            EntityDefinition = ObjectBuilder.GetObject<ISourceRepository>().LoadOneAsync<EntityDefinition>(x => x.Name == endpoint.Entity).Result;
        }



        private readonly string[] m_importDirectives =
        {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace,
            typeof(Task<>).Namespace,
            typeof(Enumerable).Namespace,
            typeof(JsonConvert).Namespace,
            "System.Web.Http",
            "System.Net.Http",
            "Bespoke.Sph.WebApi"
        };



        private Method GenerateGetAction()
        {
            var code = new StringBuilder();
            var route = this.Endpoint.GetRoute();
            var tokenRoute = route.Replace("~/", "");
            var constraints = Strings.RegexValues(route, "(?<constraint>:.*)}", "constraint");
            foreach (var cnt in constraints)
            {
                tokenRoute = tokenRoute.Replace(cnt, string.Empty);
            }

            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [QueryRoute(\"{route}\")]");

            var routeParameterFields = this.Endpoint.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>().ToArray();
            var parameterlist = from r in routeParameterFields.OrderBy(x => x.DefaultValue)
                                let uri = this.Endpoint.Route.Contains("{" + r.Name + "}") ? "" : $@"[FromUri(Name=""{r.Name}"")]"
                                select $@"
                                   {uri}{r.GenerateParameterCode()},";
            var parameters = $@"[SourceEntity(""{this.Endpoint.Id}"")]QueryEndpoint eq,
                                   [IfNoneMatch]ETag etag,
                                   [ModifiedSince]ModifiedSinceHeader modifiedSince," + parameterlist.ToString(" ");

            code.AppendLine($@"       public async Task<IHttpActionResult> GetAction({parameters}");
            code.AppendLine(@"                                   [FromUri(Name=""page"")]int page = 1,");
            code.AppendLine(@"                                   [FromUri(Name=""size"")]int size = 20)");
            code.Append("       {");


            code.Append(GenerateGetQueryCode());

            code.Append($@"

            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<{this.Endpoint.Entity}>>();
            var query = eq.QueryDsl;
            query.Aggregates.Clear();
            query.Aggregates.AddRange(aggregates);
            query.Skip = skip;
            query.Size = size;
");
            foreach (var p in routeParameterFields.Where(p => p.IsOptional))
            {
                var defaultValue = p.GenerateDefaultValueCode();
                if (!string.IsNullOrWhiteSpace(defaultValue))
                    code.AppendLine(defaultValue);
            }
            foreach (var p in routeParameterFields)
            {
                var filterCode = $@"  query.Filters.Single(x => x.Field.WebId == ""{p.WebId}"").Field = new ConstantField{{ Type = typeof({p.Type.ToCSharp()}), Value = {p.Name}, WebId = ""{p.WebId}""}};";
                code.AppendLine(filterCode);
            }
            code.AppendLine();
            code.AppendLine("var lo = await repos.SearchAsync(query);");

            code.Append(this.GenerateCacheCode());
            code.Append(this.GenerateListCode());

            code.Append($@"
                var links =  new System.Collections.Generic.List<object>();
                var nextLink = new {{
                    method = ""GET"",
                    rel = ""next"",
                    href =  $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page+1}}&size={{size}}"",
                    desc = ""Issue a GET request to get the next page of the result""                        
                }};
                var previousLink =  new {{
                    method = ""GET"",
                    rel = ""previous"",
                    href = $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page-1}}&size={{size}}""  ,  
                    desc = ""Issue a GET request to get the previous page of the result""                                  
                }};
                var hasNextPage = count > size * page;
                var hasPreviousPage = page > 1;
                
                if(hasPreviousPage){{
                    links.Add(previousLink);                
                }}

                if(hasNextPage){{
                    links.Add(nextLink);
                
                }}

                {this.GenerateServiceDescriptionLinks()}
                var result = new 
                {{
                    _results = list.Select(x => JObject.Parse(x)),
                    _count = lo.TotalRows,
                    _page = page,
                    _links = links.ToArray(),
                    _size = size
                }};
            
                return Ok(result, cache);
            ");
            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
            return new Method { Code = code.ToString() };
        }

        private string GenerateServiceDescriptionLinks()
        {
            if (!string.IsNullOrWhiteSpace(this.Endpoint.Route)) return string.Empty;
            var cacheKey = $"service-description-links-{this.Endpoint.Id}";
            var code = new StringBuilder();
            code.AppendLine($@"
            var serviceDescriptionLinks = CacheManager.Get<System.Collections.Generic.List<object>>(""{cacheKey}"");
            if( null == serviceDescriptionLinks)
            {{
                serviceDescriptionLinks = new System.Collections.Generic.List<object>();
                var context = new SphDataContext();
                var queries = context.LoadFromSources<QueryEndpoint>(x => x.Entity == ""{this.Endpoint.Entity}"" && x.Id != ""{this.Endpoint.Id}"");
                var queryLinks = from r in queries
                    select new
                    {{
                        rel = r.Name,
                        method = ""GET"",
                        href = r.GetRoute(),
                        desc = r.Note

                    }};
                serviceDescriptionLinks.AddRange(queryLinks);
                var operations = context.LoadFromSources<OperationEndpoint>(x => x.Entity == ""{this.Endpoint.Entity}"").ToArray();
                var postLinks = from r in operations
                                where r.IsHttpPost
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""POST"",
                                         href = r.Route,
                                         desc = r.Note

                                     }};
                serviceDescriptionLinks.AddRange(postLinks);
                var putLinks = from r in operations
                                where r.IsHttpPut
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""PUT"",
                                         href = $""/api/{this.Endpoint.Resource}/{{r.GetPutRoute()}}"",
                                         desc = r.Note

                                     }};
                serviceDescriptionLinks.AddRange(putLinks);
                var patchLinks = from r in operations
                                where r.IsHttpPatch
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""PATCH"",
                                         href = $""/api/{this.Endpoint.Resource}/{{r.Route}}"",
                                         desc = r.Note

                                     }};
                serviceDescriptionLinks.AddRange(patchLinks);
                var deleteLinks = from r in operations
                                where r.IsHttpDelete
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""DELETE"",
                                         href = $""/api/{this.Endpoint.Resource}/{{r.GetDeleteRoute()}}"",
                                         desc = r.Note

                                     }};
                serviceDescriptionLinks.AddRange(deleteLinks);
                CacheManager.Insert(""{cacheKey}"", serviceDescriptionLinks, $@""{{ConfigurationManager.SphSourceDirectory}}\OperationEndpoint"", $@""{{ConfigurationManager.SphSourceDirectory}}\QueryEndpoint"");
            }}
            links.AddRange(serviceDescriptionLinks);
");

            return code.ToString();
        }

        private string GenerateCacheCode()
        {

            return $@"DateTime? lastModified = new DateTime?();
            var count = lo.TotalRows;
            if (count > 0)
                lastModified = lo.GetAggregateValue<DateTime>(""LastChangedDate"");
";
        }

        private string GenerateComplexMemberFields(string[] members)
        {
            var code = new StringBuilder();
            var parent = "";
            var complexFields = members.Where(x => x.Contains(".")).OrderBy(x => x).ToList();
            foreach (var g in complexFields)
            {
                if (!(this.EntityDefinition.GetMember(g) is SimpleMember mb)) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                var paths = g.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //if (paths.Count > 2)
                //{
                //    paths.RemoveAt(paths.Count - 1);
                //    var similiar = members.Where(x => x.StartsWith(string.Join(".", paths))).ToArray();
                //    code.Append(this.GenerateComplexMemberFields(similiar));
                //    continue;
                //}

                var cp = paths.First();
                var m = paths.Last();
                if (!string.IsNullOrWhiteSpace(parent) && parent != cp) code.AppendLine("     },");
                if (parent != cp)
                {
                    code.AppendLine($"          {cp} = new {{");
                }
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $@"      {m} = (string)reader[""{g}""] ,"
                        : $@"      {m} = reader[""{g}""] != null ? ({mb.Type.ToCSharp()})reader[""{g}""] : new Nullable<{mb.Type.ToCSharp()}>(),");

                parent = cp;
            }
            if (complexFields.Count > 0) code.AppendLine("     },");

            return code.ToString();
        }

        public string GenerateListCode()
        {
            var code = new StringBuilder();
            code.AppendLine(@"var hashed = base.GetMd5Hash($""{lastModified}-{count}"");");
            code.AppendLine(@"var cache = new CacheMetadata(hashed, lastModified, setting.CachingSetting);");

            if (!this.Endpoint.MemberCollection.Any())
            {
                code.Append(@" 
                    var list = from f in lo.ItemCollection
                               let link = $""\""link\"" :{{ \""href\"" :\""{ConfigurationManager.BaseUrl}/api/" + this.Endpoint.Resource + @"/{f.Id}\""}}""
                               select f.ToJson().Replace($""{f.WebId}\"""",$""{f.WebId}\"","" + link);
");
                return code.ToString();
            }

            code.Append(@"
            var list = from reader in lo.Readers
                        let id =  (string)reader[""Id""]
                        select JsonConvert.SerializeObject( new {");
            foreach (var g in this.Endpoint.MemberCollection.Where(x => !x.Contains(".")))
            {
                if (!(this.EntityDefinition.GetMember(g) is SimpleMember mb)) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $@"      {g} = reader[""{g}""],"
                        : $@"      {g} = reader[""{g}""] != null ? ({mb.Type.ToCSharp()})reader[""{g}""] :new Nullable<{mb.Type.ToCSharp()}>(), ");
            }
            code.Append(this.GenerateComplexMemberFields(this.Endpoint.MemberCollection.ToArray()));

            code.Append($@"
                            _links = new {{
                                rel = ""self"",
                                href = $""{{ConfigurationManager.BaseUrl}}/api/{this.Endpoint.Resource}/{{id}}""
                            }}
                        }});
");
            return code.ToString();
        }

        public async Task<Class> GetAssemblyInfoCodeAsync()
        {
            var info = await AssemblyInfoClass.GenerateAssemblyInfoAsync(this.Endpoint, true, this.Endpoint.Name);
            return info.ToClass();


        }

        public Class GenerateCode()
        {

            var controller = new Class
            {
                Name = this.Endpoint.TypeName,
                IsPartial = true,
                FileName = $"{this.Endpoint.TypeName}.cs",
                BaseClass = "BaseApiController",
                Namespace = this.Endpoint.CodeNamespace
            };


            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(this.EntityDefinition.CodeNamespace);
            controller.ImportCollection.Add(typeof(List<string>).Namespace);

            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.AddProperty($"public static readonly string SOURCE_FILE = $\"{{ConfigurationManager.SphSourceDirectory}}\\\\{nameof(QueryEndpoint)}\\\\{this.Endpoint.Id}.json\";");
            controller.AddProperty($"public const string CACHE_KEY = \"entity-query:{this.Endpoint.Id}\";");
            controller.AddProperty($"public const string ES_QUERY_CACHE_KEY = \"entity-query:es-query:{this.Endpoint.Id}\";");
            controller.CtorCollection.Add($"public {this.Endpoint.TypeName}() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");

            controller.MethodCollection.Add(this.GenerateGetAction());
            controller.MethodCollection.Add(GenerateCountAction());

            return controller;
        }



        private Method GenerateCountAction()
        {
            var route = $"{this.Endpoint.GetRoute()}/_metadata/_count".Replace("//", "/");

            var code = $@"  
        [HttpGet]
        [GetRoute(""{route}"")]
        public async Task<IHttpActionResult> GetCountAsync([SourceEntity(""{this.Endpoint.Id}"")]QueryEndpoint eq,
                                                     [IfNoneMatch]ETag etag,
                                                     [ModifiedSince]ModifiedSinceHeader modifiedSince)
        {{
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<{this.Endpoint.Entity}>>();
            var count = await repos.GetCountAsync(eq.FilterCollection.ToArray());
            return Ok(new {{ _count = count }});

        }}";


            return new Method { Code = code };
        }

        private string GenerateGetQueryCode()
        {

            return @"   
            var setting = await eq.LoadSettingAsync();
            var skip = size * (page - 1);
            var aggregates = new List<Aggregate>{
              new MaxAggregate(name:""LastChangedDate"", path:""ChangedDate"")
            };";
        }
    }
}