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

            var action = GenerateAction();
            controller.MethodCollection.Add(action);

            return controller;
        }


        private Method GenerateAction()
        {
            var code = new StringBuilder();

            var route = this.Route.StartsWith("~") ? this.Route : $"~/api/{Entity.ToLowerInvariant()}/{this.Route}";
            var tokenRoute = this.Route.StartsWith("~")
                ? route.Replace("~/", "")
                : $"api/{Entity.ToLowerInvariant()}/{this.Route}";
            code.AppendLine("       [HttpGet]");
            code.AppendLine($"       [Route(\"{route}\")]");
            code.AppendLine("       public async Task<ActionResult> GetAction(int page =1, int size=20)");
            code.Append("       {");
            code.Append($@"
            var eq = CacheManager.Default.Get<EntityQuery>(""entity-query-{Id}"");
            if(null == eq )
            {{
                var context = new SphDataContext();
                eq = await context.LoadOneAsync<EntityQuery>(x => x.Id == ""{Id}"");
                CacheManager.Default.Insert(""entity-query-{Id}"", eq, $""{{ConfigurationManager.SphSourceDirectory}}\\EntityQuery\\{Id}.json"");
            }}
            var ds = ObjectBuilder.GetObject<IDirectoryService>();
            var query = await eq.GenerateEsQueryAsync(page, size);
            var request = new StringContent(query);
            var url = ""{ConfigurationManager.ApplicationName.ToLower()}/{this.Entity.ToLower()}/_search"";

            using(var client = new HttpClient{{BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost)}})
            {{
                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);

                var json = await content.ReadAsStringAsync();
                var jo = JObject.Parse(json);
");

            var context = new SphDataContext();
            var ed = context.LoadOne<EntityDefinition>(x => x.Name == this.Entity);
            code.Append(this.GenerateListCode(ed));
      
            code.Append($@"
                var result = new 
                {{
                    results = ""<list>"",
                    rows = jo.SelectToken(""$.hits.total"").Value<int>(),
                    page = page,
                    nextPageToken = $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page+1}}&size={{size}}"",
                    previousPageToken = page == 1 ? null : $""{{ConfigurationManager.BaseUrl}}/{tokenRoute}?page={{page-1}}&size={{size}}"",
                    size = size
                }};
                var tempResult = JsonConvert.SerializeObject(result);
                var temp = string.Join("", "", list);
            
                return Content(tempResult.Replace(""\""<list>\"""",""[""+ temp + ""]""), ""application/json"");
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