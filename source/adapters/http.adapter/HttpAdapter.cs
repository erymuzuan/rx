using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "HTTP adapter", FontAwesomeIcon = "html5", Route = "adapter.http/0", RouteTableProvider = typeof(HttpAdapterRouteTableProvider))]
    public partial class HttpAdapter : Adapter
    {
        public static readonly string[] ImportDirectives =
        {
    typeof(Entity).Namespace,
    typeof(Int32).Namespace ,
    typeof(Task<>).Namespace ,
    typeof(Enumerable).Namespace ,
    typeof(IEnumerable<>).Namespace,
    typeof(HttpClient).Namespace ,
    typeof(Encoding).Namespace ,
    typeof(CookieContainer).Namespace,
    typeof(XmlAttributeAttribute).Namespace ,
    "System.Web.Mvc",
    "Bespoke.Sph.Web.Helpers"

        };

        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(HttpClient));

            var code = new Class { Name = Name, BaseClass = nameof(IDisposable), Namespace = CodeNamespace };
            code.ImportCollection.AddRange(namespaces);
            code.ImportCollection.AddRange(ImportDirectives);
            var sources = new ObjectCollection<Class> { code };

            code.AddProperty("       public bool IsAuthenticated {get;set;}");
            code.AddProperty("       private HttpClient m_client;");
            code.AddProperty("       private CookieContainer m_cookieContainer = new CookieContainer();");
            code.AddProperty("       const string BASE_ADDRESS = \"{0}\";", this.BaseAddress);

            var login = AddLoginSource();
            if (null != login)
                code.PropertyCollection.Add(login);

            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = $"{op.HttpMethod}_{op.Name}_";

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);

                //
                code.AddMethod(op.GenerateActionCode(this, methodName));

                var requestSources = op.GenerateRequestCode();
                sources.AddRange(requestSources);

                var responseSources = op.GenerateResponseCode();
                sources.AddRange(responseSources);
            }
            code.CtorCollection.Add(GenerateCtorCode());
            code.MethodCollection.Add(GenerateDisposableMethod());
            return Task.FromResult(sources.AsEnumerable());
        }

        private string GenerateCtorCode()
        {

            var code = new StringBuilder();
            code.AppendLinf("       public {0}()", this.Name);
            code.AppendLine("       {");
            code.AppendLine("           var handler = new HttpClientHandler { CookieContainer = m_cookieContainer };");
            code.AppendLine("           if (handler.SupportsAutomaticDecompression)");
            code.AppendLine("           {");
            code.AppendLine("               handler.AutomaticDecompression = DecompressionMethods.GZip |DecompressionMethods.Deflate;");
            code.AppendLine("           }");
            code.AppendLine("           m_client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)};");

            if (this.Timeout.HasValue)
                code.AppendLine($"           m_client.Timeout = TimeSpan.From{TimeoutInterval}({Timeout});");

            code.AppendLine("       }");


            return code.ToString();
        }

        private Method GenerateDisposableMethod()
        {
            var code = new StringBuilder();
            code.AppendLine("       public void Dispose()");
            code.AppendLine("       {");
            code.AppendLine("           m_client.Dispose();");

            code.AppendLine("       }");

            return new Method { Code = code.ToString() };
        }

        private Property AddLoginSource()
        {
            if (this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Any(o => o.IsLoginRequired) &&
                this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Count(o => o.IsLoginOperation) != 1)
            {
                throw new InvalidOperationException("You have to have a user login(authentication) operation");
            }
            if (this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Any(o => o.IsLoginRequired))
            {
                var login = this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Single(a => a.IsLoginOperation);
                var name = (login.HttpMethod + "_" + login.Name).ToCsharpIdentitfier();
                var code = $"       public {name}Request LoginCredential {{get;set;}}";

                return new Property { Code = code };
            }
            return null;
        }


        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            return Task.FromResult(default(Class));
        }

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            return Task.FromResult(default(Class));
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new NotImplementedException();
        }

        public override string OdataTranslator => null;


        public Task OpenAsync()
        {
            var jo = JObject.Parse(File.ReadAllText(this.Har));
            var entries = jo.SelectTokens("$.log.entries").SelectMany(x => x).ToArray();
            var operations = from j in entries
                             let url = j.SelectToken("request.url").Value<string>()
                             let uri = new Uri(url)
                             select new HttpOperationDefinition(j)
                             {
                                 Url = uri.AbsolutePath,
                                 HttpMethod = j.SelectToken("request.method").Value<string>(),
                                 Uuid = Guid.NewGuid().ToString()
                             };

            this.OperationDefinitionCollection.AddRange(operations);
            var urls = from j in entries
                       let url = j.SelectToken("request.url").Value<string>()
                       let uri = new Uri(url)
                       select uri;
            var ur = urls.First();
            this.BaseAddress = $"{ur.Scheme}://{ur.Host}{(ur.IsDefaultPort ? "" : ":" + ur.Port)}";
            return Task.FromResult(0);
        }
    }
}
