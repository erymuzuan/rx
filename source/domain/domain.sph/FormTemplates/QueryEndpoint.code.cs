using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class QueryEndpoint
    {
        private EntityDefinition m_ed;
        public override string ToString()
        {
            return this.Name;
        }
        private readonly string[] m_importDirectives =
        {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(JsonConvert).Namespace ,
            "System.Web.Http",
            "System.Net.Http",
            "Bespoke.Sph.WebApi"
        };

        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.Api";
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.QueryEndpoint.{Entity}.{Id}.dll";
        [JsonIgnore]
        public string PdbName => $"{ConfigurationManager.ApplicationName}.QueryEndpoint.{Entity}.{Id}.pdb";
        [JsonIgnore]
        public string TypeName => $"{ControllerName}Controller";
        public string ControllerName => $"{Entity}{Name.ToPascalCase()}QueryEndpoint";
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";


        public string[] SaveSources(IEnumerable<Class> classes)
        {
            var sources = classes.ToArray();
            var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources)
            {
                var file = Path.Combine(folder, cs.FileName);
                File.WriteAllText(file, cs.GetCode());
            }

            var assemblyInfo = AssemblyInfoClass.GenerateAssemblyInfoAsync(this, true, this.Name).Result;

            return sources
                .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{this.Name}\\{f.FileName}")
                .Concat(new[] { assemblyInfo.FileName })
                .ToArray();
        }

        public Class GenerateCode(EntityDefinition ed)
        {
            m_ed = ed ?? throw new ArgumentNullException(nameof(ed), "Ed cannot be null");

            var controller = new Class
            {
                Name = TypeName,
                IsPartial = true,
                FileName = $"{TypeName}.cs",
                BaseClass = "BaseApiController",
                Namespace = CodeNamespace
            };


            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(ed.CodeNamespace);
            controller.ImportCollection.Add(typeof(List<string>).Namespace);

            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.AddProperty($"public static readonly string SOURCE_FILE = $\"{{ConfigurationManager.SphSourceDirectory}}\\\\{nameof(QueryEndpoint)}\\\\{Id}.json\";");
            controller.AddProperty($"public const string CACHE_KEY = \"entity-query:{Id}\";");
            controller.AddProperty($"public const string ES_QUERY_CACHE_KEY = \"entity-query:es-query:{Id}\";");
            controller.CtorCollection.Add($"public {TypeName}() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");

            controller.MethodCollection.Add(GenerateGetAction());
            controller.MethodCollection.Add(GenerateCountAction());

            return controller;
        }

        public string GetRoute()
        {
            if (string.IsNullOrWhiteSpace(this.Resource))
                this.Resource = this.Entity.Pluralize().ToIdFormat();
            var parameters = Strings.RegexValues(this.Route, "\\{(?<p>.*?)}", "p");
            var route = this.Route;
            foreach (var rp in parameters)
            {
                var field = this.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>()
                    .Single(x => x.Name == rp);
                route = route.Replace($"{{{rp}}}", $"{{{rp}{field.GetRouteConstraint()}}}");
            }
            return this.Route.StartsWith("~") ? route : $"~/api/{Resource}/{route}";
        }
        public string GetLocation()
        {
            var route = this.GetRoute();
            if (route.StartsWith("~/")) route = route.Replace("~/", "/");
            return route;
        }


        private Method GenerateCountAction()
        {
            var route = $"{this.GetRoute()}/_metadata/_count".Replace("//", "/");

            var code = $@"  
        [HttpGet]
        [GetRoute(""{route}"")]
        public async Task<IHttpActionResult> GetCountAsync([SourceEntity(""{Id}"")]QueryEndpoint eq,
                                                     [IfNoneMatch]ETag etag,
                                                     [ModifiedSince]ModifiedSinceHeader modifiedSince)
        {{
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{Entity}>>();
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

        public Query Query
        {
            get
            {
                var query = new Query(this.FilterCollection.ToArray(), this.SortCollection.ToArray());
                if (this.MemberCollection.Any())
                    query.Fields.AddRange(this.MemberCollection);
                return query;
            }
        }


        private Method GenerateGetAction()
        {
            var code = new StringBuilder();
            var route = this.GetRoute();
            var tokenRoute = route.Replace("~/", "");
            var constraints = Strings.RegexValues(route, "(?<constraint>:.*)}", "constraint");
            foreach (var cnt in constraints)
            {
                tokenRoute = tokenRoute.Replace(cnt, string.Empty);
            }

            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [QueryRoute(\"{route}\")]");

            var routeParameterFields = this.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>().ToArray();
            var parameterlist = from r in routeParameterFields.OrderBy(x => x.DefaultValue)
                                let uri = this.Route.Contains("{" + r.Name + "}") ? "" : $@"[FromUri(Name=""{r.Name}"")]"
                                select $@"
                                   {uri}{r.GenerateParameterCode()},";
            var parameters = $@"[SourceEntity(""{Id}"")]QueryEndpoint eq,
                                   [IfNoneMatch]ETag etag,
                                   [ModifiedSince]ModifiedSinceHeader modifiedSince," + parameterlist.ToString(" ");

            code.AppendLine($@"       public async Task<IHttpActionResult> GetAction({parameters}");
            code.AppendLine(@"                                   [FromUri(Name=""page"")]int page = 1,");
            code.AppendLine(@"                                   [FromUri(Name=""size"")]int size = 20)");
            code.Append("       {");


            code.Append(GenerateGetQueryCode());

            code.Append($@"

            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{Entity}>>();
            var qr = eq.Query.Clone();
            qr.Aggregates.Clear();
            qr.Aggregates.AddRange(aggregates);
            qr.Skip = skip;
            qr.Size = size;
");
            foreach (var p in routeParameterFields.Where(p => p.IsOptional))
            {
                var defaultValue = p.GenerateDefaultValueCode();
                if (!string.IsNullOrWhiteSpace(defaultValue))
                    code.AppendLine(defaultValue);
            }
            foreach (var p in routeParameterFields)
            {
                var filterCode = $@"  qr.Filters.Single(x => x.Field.WebId == ""{p.WebId}"").Field = new ConstantField{{ Type = typeof({p.Type.ToCSharp()}), Value = {p.Name}, WebId = ""{p.WebId}""}};";
                code.AppendLine(filterCode);
            }
            code.AppendLine();
            code.AppendLine("var lo = await repos.SearchAsync(qr);");

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
            if (!string.IsNullOrWhiteSpace(this.Route)) return string.Empty;
            var cacheKey = $"service-description-links-{Id}";
            var code = new StringBuilder();
            code.AppendLine($@"
            var sericeDescriptionLinks = CacheManager.Get<System.Collections.Generic.List<object>>(""{cacheKey}"");
            if( null == sericeDescriptionLinks)
            {{
                sericeDescriptionLinks = new System.Collections.Generic.List<object>();
                var context = new SphDataContext();
                var queries = context.LoadFromSources<QueryEndpoint>(x => x.Entity == ""{Entity}"" && x.Id != ""{Id}"");
                var queryLinks = from r in queries
                    select new
                    {{
                        rel = r.Name,
                        method = ""GET"",
                        href = r.GetRoute(),
                        desc = r.Note

                    }};
                sericeDescriptionLinks.AddRange(queryLinks);
                var operations = context.LoadFromSources<OperationEndpoint>(x => x.Entity == ""{Entity}"").ToArray();
                var postLinks = from r in operations
                                where r.IsHttpPost
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""POST"",
                                         href = r.Route,
                                         desc = r.Note

                                     }};
                sericeDescriptionLinks.AddRange(postLinks);
                var putLinks = from r in operations
                                where r.IsHttpPut
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""PUT"",
                                         href = $""/api/{Resource}/{{r.GetPutRoute()}}"",
                                         desc = r.Note

                                     }};
                sericeDescriptionLinks.AddRange(putLinks);
                var patchLinks = from r in operations
                                where r.IsHttpPatch
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""PATCH"",
                                         href = $""/api/{Resource}/{{r.Route}}"",
                                         desc = r.Note

                                     }};
                sericeDescriptionLinks.AddRange(patchLinks);
                var deleteLinks = from r in operations
                                where r.IsHttpDelete
                                     select new
                                     {{
                                         rel = r.Name,
                                         method = ""DELETE"",
                                         href = $""/api/{Resource}/{{r.GetDeleteRoute()}}"",
                                         desc = r.Note

                                     }};
                sericeDescriptionLinks.AddRange(deleteLinks);
                CacheManager.Insert(""{cacheKey}"", sericeDescriptionLinks, $@""{{ConfigurationManager.SphSourceDirectory}}\OperationEndpoint"", $@""{{ConfigurationManager.SphSourceDirectory}}\QueryEndpoint"");
            }}
            links.AddRange(sericeDescriptionLinks);
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


        public string GenerateListCode()
        {
            var code = new StringBuilder();
            code.AppendLine(@"var hashed = base.GetMd5Hash($""{lastModified}-{count}"");");
            code.AppendLine(@"var cache = new CacheMetadata(hashed, lastModified, setting.CachingSetting);");

            if (!this.MemberCollection.Any())
            {
                code.Append(@" 
                    var list = from f in lo.ItemCollection
                               let link = $""\""link\"" :{{ \""href\"" :\""{ConfigurationManager.BaseUrl}/api/" + this.Resource + @"/{f.Id}\""}}""
                               select f.ToJsonString().Replace($""{f.WebId}\"""",$""{f.WebId}\"","" + link);
");
                return code.ToString();
            }

            code.Append(@"
            var list = from reader in lo.Readers
                        let id =  (string)reader[""Id""]
                        select JsonConvert.SerializeObject( new {");
            foreach (var g in this.MemberCollection.Where(x => !x.Contains(".")))
            {
                if (!(m_ed.GetMember(g) is SimpleMember mb)) throw new InvalidOperationException("You can only select SimpleMember field, and " + g + " is not");
                code.AppendLine(
                    mb.Type == typeof(string)
                        ? $@"      {g} = reader[""{g}""],"
                        : $@"      {g} = reader[""{g}""] != null ? ({mb.Type.ToCSharp()})reader[""{g}""] :new Nullable<{mb.Type.ToCSharp()}>(), ");
            }
            code.Append(this.GenerateComplexMemberFields(this.MemberCollection.ToArray()));

            code.Append($@"
                            _links = new {{
                                rel = ""self"",
                                href = $""{{ConfigurationManager.BaseUrl}}/api/{Resource}/{{id}}""
                            }}
                        }});
");
            return code.ToString();
        }


        public Task<WorkflowCompilerResult> CompileAsync(EntityDefinition ed)
        {
            var options = new CompilerOptions();
            var sources = this.GenerateCode(ed);
            var result = this.Compile(options, sources);

            return Task.FromResult(result);
        }
        public WorkflowCompilerResult Compile(CompilerOptions options, params Class[] classes)
        {
            if (classes.Length == 0)
                throw new ArgumentException("No files", nameof(classes));

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, this.AssemblyName),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.AddReference(typeof(Entity),
                    typeof(int),
                    typeof(INotifyPropertyChanged),
                    typeof(Expression<>),
                    typeof(XmlAttributeAttribute),
                    typeof(SmtpClient),
                    typeof(HttpClient),
                    typeof(XElement),
                    typeof(HttpResponseBase),
                    typeof(ConfigurationManager));

                foreach (var es in options.EmbeddedResourceCollection)
                {
                    parameters.EmbeddedResources.Add(es);
                }
                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    parameters.ReferencedAssemblies.Add(ass);
                }

                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\webapi.common.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Http.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.CompilerOutputPath + $@"\{ConfigurationManager.ApplicationName}.{Entity}.dll");

                var folder = $"{ConfigurationManager.GeneratedSourceDirectory}\\QueryEndpoint.{Entity}.{this.Name}";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                classes.ToList().ForEach(x =>
                {
                    var file = $"{folder}\\{x.FileName}";
                    File.WriteAllText(file, x.GetCode());
                });

                var assemblyInfo = AssemblyInfoClass.GenerateAssemblyInfoAsync(this, true, $"QueryEndpoint.{Entity}.{this.Name}").Result;

                var files = classes.Select(x => $"{folder}\\{x.FileName}")
                    .Concat(new[] { assemblyInfo.FileName })
                    .ToArray();
                var result = provider.CompileAssemblyFromFile(parameters, files);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                             select new BuildError(this.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return cr;
            }
        }


    }
}