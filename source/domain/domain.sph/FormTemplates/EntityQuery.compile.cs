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
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityQuery
    {
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
        public Class GenerateCode()
        {
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


            controller.PropertyCollection.Add(new Property {Name = "CacheManager", Type = typeof(ICacheManager)});
            controller.CtorCollection.Add($"public {className}Controller() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");

            controller.MethodCollection.Add(GenerateGetAction());
            controller.MethodCollection.Add(GenerateCountAction());

            return controller;
        }


        private Method GenerateCountAction()
        {
            var code = new StringBuilder();
            var route = this.Route.StartsWith("~") ? this.Route : $"~/api/{Entity.ToLowerInvariant()}/{this.Route}";

            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [Route(\"{route}/_count\")]");
            code.AppendLine("       public async Task<ActionResult> GetCountAsync()");
            code.Append("       {");
            code.Append($@"
            var eq = CacheManager.Get<EntityQuery>(""entity-query:{Id}"");
            var sourceFile = $""{{ConfigurationManager.SphSourceDirectory}}\\EntityQuery\\{Id}.json"";
            if(null == eq )
            {{
                var context = new SphDataContext();
                eq = await context.LoadOneAsync<EntityQuery>(x => x.Id == ""{Id}"");
                CacheManager.Insert(""entity-query:{Id}"", eq, sourceFile);
            }}");

            if (this.CacheFilter.HasValue)
            {
                code.AppendLine($"   var queryCacheKey = $\"entity-query:filter:{Id}\";");
                code.AppendLine($"   var query = CacheManager.Get<string>(queryCacheKey);");
                code.AppendLine("   if(null == query)");
                code.AppendLine("   {");
                code.AppendLine("       query = await eq.GenerateEsQueryAsync(1, 20);");
                code.AppendLine($"       CacheManager.Insert(queryCacheKey, query, TimeSpan.FromSeconds({this.CacheFilter}), sourceFile);");
                code.AppendLine("   }");
            }
            else
            {
                code.AppendLine("var query = await eq.GenerateEsQueryAsync(1, 20);");
            }
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


        private Method GenerateGetAction()
        {
            var code = new StringBuilder();

            var route = this.Route.StartsWith("~") ? this.Route : $"~/api/{Entity.ToLowerInvariant()}/{this.Route}";
            var tokenRoute = this.Route.StartsWith("~")
                ? route.Replace("~/", "")
                : $"api/{Entity.ToLowerInvariant()}/{this.Route}";
            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [Route(\"{route}\")]");

            var parameterlist = from r in this.RouteParameterCollection
                let defaultValue = string.IsNullOrWhiteSpace(r.DefaultValue) ? "" : $" = {r.DefaultValue}"
                let type = r.Type.ToCsharpIdentitfier()
                select $"{type} {r.Name}{defaultValue},";
            var parameters = string.Join(" ", parameterlist);

            code.AppendLine($"       public async Task<ActionResult> GetAction({parameters}int page =1, int size=20)");
            code.Append("       {");
            code.Append($@"
            var eq = CacheManager.Get<EntityQuery>(""entity-query:{Id}"");
            var sourceFile = $""{{ConfigurationManager.SphSourceDirectory}}\\EntityQuery\\{Id}.json"";
            if(null == eq )
            {{
                var context = new SphDataContext();
                eq = await context.LoadOneAsync<EntityQuery>(x => x.Id == ""{Id}"");
                CacheManager.Insert(""entity-query:{Id}"", eq, sourceFile);
            }}");

            if (this.CacheFilter.HasValue)
            {
                code.AppendLine($"   var queryCacheKey = $\"entity-query:filter:{{page}}:{{size}}:{Id}\";");
                code.AppendLine($"   var query = CacheManager.Get<string>(queryCacheKey);");
                code.AppendLine("   if(null == query)");
                code.AppendLine("   {");
                code.AppendLine("       query = await eq.GenerateEsQueryAsync(page, size);");
                code.AppendLine($"       CacheManager.Insert(queryCacheKey, query, TimeSpan.FromSeconds({this.CacheFilter}), sourceFile);");
                code.AppendLine("   }");
            }
            else
            {
                code.AppendLine("var query = await eq.GenerateEsQueryAsync(page, size);");
            }

            foreach (var p in this.FilterCollection.Select(x => x.Field).OfType<RouteParameterField>())
            {
                code.AppendLine($"  query = query.Replace(\"<<{p.Expression}>>\", {p.Expression});");
            }
            code.Append($@"
            var request = new StringContent(query);
            var url = ""{ConfigurationManager.ApplicationName.ToLower()}/{this.Entity.ToLower()}/_search"";

            using(var client = new HttpClient{{BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost)}})
            {{
                var esResponse = await client.PostAsync(url, request);
                var esResponseContent = esResponse.Content as StreamContent;
                if (null == esResponseContent) throw new Exception(""Cannot execute query on es "" + request);

                var esResponseString = await esResponseContent.ReadAsStringAsync();
                var esJsonObject = JObject.Parse(esResponseString);
");

            var context = new SphDataContext();
            var ed = context.LoadOne<EntityDefinition>(x => x.Name == this.Entity);
            code.Append(this.GenerateListCode(ed));
      
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