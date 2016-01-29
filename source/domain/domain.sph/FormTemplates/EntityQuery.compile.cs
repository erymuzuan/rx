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
    public partial class EntityQuery
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
            "System.Web.Mvc",
            "System.Net.Http",
            "Bespoke.Sph.Web.Helpers"
        };

        public string CodeNamespace => $"Bespoke.{ConfigurationManager.ApplicationName}.Api";


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
            return sources
                    .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{this.Name}\\{f.FileName}")
                    .ToArray();
        }

        public Class GenerateCode(EntityDefinition ed)
        {
            if (null == ed)
                throw new ArgumentNullException(nameof(ed), "Ed cannot be null");
            m_ed = ed;

            var className = this.Name.ToPascalCase();
            var controller = new Class
            {
                Name = $"{className}Controller",
                IsPartial = true,
                FileName = $"{className}Controller.cs",
                BaseClass = "Controller",
                Namespace = CodeNamespace
            };


            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");


            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.AddProperty($"public static readonly string SOURCE_FILE = $\"{{ConfigurationManager.SphSourceDirectory}}\\\\EntityQuery\\\\{Id}.json\";");
            controller.AddProperty($"public const string CACHE_KEY = \"entity-query:{Id}\";");
            controller.AddProperty($"public const string ES_QUERY_CACHE_KEY = \"entity-query:es-query:{Id}\";");
            controller.CtorCollection.Add($"public {className}Controller() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");

            controller.MethodCollection.Add(GenerateGetAction());
            controller.MethodCollection.Add(GenerateCountAction());

            return controller;
        }

        public string GetRoute()
        {
            if (string.IsNullOrWhiteSpace(this.Resource))
                this.Resource = this.Entity.Pluralize();
            return this.Route.StartsWith("~") ? this.Route : $"~/api/{Resource}/{this.Route}";
        }
        public string GetLocation()
        {
            var route = this.GetRoute();
            if (route.StartsWith("~/")) route = route.Replace("~/", "/");
            return route;
        }


        private Method GenerateCountAction()
        {
            var code = new StringBuilder();
            var route = this.GetRoute();

            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [Route(\"{route}/_count\")]");
            code.AppendLine("       public async Task<ActionResult> GetCountAsync()");
            code.Append("       {");
            code.Append(GenerateGetQueryCode());

            code.Append($@"
            var request = new StringContent(query);
            var url = ""{ConfigurationManager.ApplicationName.ToLower()}/{this.Entity.ToLower()}/_count"";

            using(var client = new HttpClient{{BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost)}})
            {{
                var esResponse = await client.PostAsync(url, request);
                var esResponseContent = esResponse.Content as StreamContent;
                if (null == esResponseContent) throw new Exception(""Cannot execute query on es "" + request);

                var esResponseString = await esResponseContent.ReadAsStringAsync();
                var esJsonObject = JObject.Parse(esResponseString);

                var count = esJsonObject.SelectToken(""$.count"").Value<int>();
            ");
            code.AppendLine("return Content($\"{{\\\"_count\\\":{count}}}\", \"application/json\");");

            code.Append("}");
            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
            return new Method { Code = code.ToString() };
        }

        private string GenerateGetQueryCode()
        {
            var code = new StringBuilder();
            code.Append(
                $@"
            var eq = CacheManager.Get<EntityQuery>(CACHE_KEY);
            if(null == eq )
            {{
                var context = new SphDataContext();
                eq = await context.LoadOneAsync<EntityQuery>(x => x.Id == ""{Id}"");
                CacheManager.Insert(CACHE_KEY, eq, SOURCE_FILE);
            }}");


            code.AppendLine("   var setting = await eq.LoadSettingAsync();");
            code.AppendLine("   var query = CacheManager.Get<string>(ES_QUERY_CACHE_KEY);");
            code.AppendLine("   if(null == query)");
            code.AppendLine("   {");
            code.AppendLine("       query = await eq.GenerateEsQueryAsync();");
            code.AppendLine("   }");
            code.AppendLine("   if(setting.CacheFilter.HasValue)");
            code.AppendLine("   {");
            code.AppendLine("       CacheManager.Insert(ES_QUERY_CACHE_KEY, query, TimeSpan.FromSeconds(setting.CacheFilter.Value), SOURCE_FILE);");
            code.AppendLine("   }");

            return code.ToString();
        }


        private Method GenerateGetAction()
        {
            var code = new StringBuilder();
            var route = this.GetRoute();
            var tokenRoute = route.Replace("~/", "");

            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [Route(\"{route}\")]");

            var parameterlist = from r in this.RouteParameterCollection
                                let defaultValue = string.IsNullOrWhiteSpace(r.DefaultValue) ? "" : $" = {r.DefaultValue}"
                                let type = r.Type.ToCsharpIdentitfier()
                                select $"{type} {r.Name}{defaultValue},";
            var parameters = string.Join(" ", parameterlist);

            code.AppendLine($"       public async Task<ActionResult> GetAction({parameters}int page =1, int size=20, string q=\"\")");
            code.Append("       {");
            code.Append(GenerateGetQueryCode());

            foreach (var p in this.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>())
            {
                var qs = this.RouteParameterCollection.Single(x => x.Name == p.Expression);
                code.AppendLine(qs.Type == "DateTime"
                    ? $"  query = query.Replace(\"<<{p.Expression}>>\", $\"\\\"{{{p.Expression}:O}}\\\"\");"
                    : $"  query = query.Replace(\"<<{p.Expression}>>\", $\"{{{p.Expression}}}\");");
            }
            code.Append($@"
            var request = new StringContent(query);
            var url = $""{ConfigurationManager.ApplicationName.ToLower()}/{this.Entity.ToLower()}/_search?from={{size * (page - 1)}}&size={{size}}"";
            if(!string.IsNullOrWhiteSpace(q)) url +=""&q="" + q;

            using(var client = new HttpClient{{BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost)}})
            {{
                var esResponse = await client.PostAsync(url, request);
                var esResponseContent = esResponse.Content as StreamContent;
                if (null == esResponseContent) throw new Exception(""Cannot execute query on es "" + request);

                var esResponseString = await esResponseContent.ReadAsStringAsync();
                var esJsonObject = JObject.Parse(esResponseString);
");
            
            code.Append(this.GenerateListCode());

            code.Append($@"
                var result = new 
                {{
                    _results = ""<<list>>"",
                    _count = esJsonObject.SelectToken(""$.hits.total"").Value<int>(),
                    _page = page,
                    _links = new {{
                        _next = $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page+1}}&size={{size}}"",
                        _previous = page == 1 ? null : $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page-1}}&size={{size}}""
                    }},
                    _size = size
                }};
                var resultJson = JsonConvert.SerializeObject(result);
                var itemsList = string.Join("", "", list);
            
                return Content(resultJson.Replace(""\""<<list>>\"""",""[""+ itemsList + ""]""), ""application/json"");
            }}");
            code.AppendLine();
            code.AppendLine("       }");
            code.AppendLine();
            return new Method { Code = code.ToString() };
        }

        public WorkflowCompilerResult Compile(CompilerOptions options, params Class[] @classes)
        {
            if (@classes.Length == 0)
                throw new ArgumentException(@"No files", nameof(@classes));

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"{ConfigurationManager.ApplicationName}.EntityQuery.{Id}.dll"),
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

                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");

                var folder = $"{ConfigurationManager.GeneratedSourceDirectory}\\EntityQuery.{this.Name}";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                @classes.ToList().ForEach(x =>
                {
                    var file = $"{folder}\\{x.FileName}";
                    File.WriteAllText(file, x.GetCode());
                });
                var files = @classes.Select(x => $"{folder}\\{x.FileName}").ToArray();
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