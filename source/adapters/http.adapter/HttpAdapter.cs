using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    public partial class HttpAdapter : Adapter
    {
        private string GetCodeHeader(params string[] namespaces)
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(HttpClient).Namespace + ";");
            header.AppendLine("using " + typeof(Encoding).Namespace + ";");
            header.AppendLine("using " + typeof(CookieContainer).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            foreach (var ns in namespaces)
            {
                header.AppendLinf("using {0};", ns);
            }
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }
        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            options.AddReference(typeof(HttpClient));
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name);
            code.AppendLine("   {");
            code.AppendLine("       private CookieContainer m_cookieContainer = new CookieContainer();");
            code.AppendLinf("       const string BASE_ADDRESS = \"{0}\";", this.BaseAddress);

            // login page
            if (this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Any(o => o.IsLoginRequired) &&
                this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Count(o => o.IsLoginOperation) != 1)
            {
                throw new InvalidOperationException("You have to have a user login(authentication) operation");
            }
            if (this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Any(o => o.IsLoginRequired))
            {
                var login = this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Single(a => a.IsLoginOperation);
                code.AppendLinf("       public {0}Request LoginCredential {{get;set;}}", (login.HttpMethod + "_" + login.Name).ToCsharpIdentitfier());
            }

            var added = new List<string>();
            foreach (var op in this.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                var name = op.Name;
                var methodName = string.Format("{0}_{1}_", op.HttpMethod, op.Name);
                if (added.Contains(methodName))
                    continue;
                added.Add(methodName);

                var adapterName = name + "Adapter";
                if (sources.ContainsKey(adapterName + ".cs")) continue;
                code.AppendLinf("       public async Task<{0}Response> {0}Async({0}Request request)", methodName.ToCsharpIdentitfier());
                code.AppendLine("       {");
                code.AppendLinf("           const string url = \"{0}\";", op.Url.Replace(BaseAddress, ""));

                if (op.IsLoginRequired)
                {
                    var login = this.OperationDefinitionCollection.OfType<HttpOperationDefinition>().Single(a => a.IsLoginOperation);
                    code.AppendLinf("           await this.{0}Async(this.LoginCredential);", (login.HttpMethod + "_" + login.Name).ToCsharpIdentitfier());
                }

                code.AppendLine("           using (var handler = new HttpClientHandler { CookieContainer = m_cookieContainer })");
                if (op.Timeout.HasValue)
                    code.AppendLinf("           using(var client = new HttpClient(handler){{BaseAddress = new Uri(BASE_ADDRESS), Timeout = {0}})", op.Timeout);
                else
                    code.AppendLine("           using(var client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)})");

                code.AppendLine("           {");

                var sendCode = HttpClientSendCodeGenerator.Create(op).GenerateCode(op);
                foreach (var c in sendCode.Split(new[] { Environment.NewLine, "\r\n", "\n" }, StringSplitOptions.None))
                {
                    code.AppendLine("               " + c);
                }

                code.AppendLinf("               var result =  new {0}Response();", methodName.ToCsharpIdentitfier());
                code.AppendLine("               await result.LoadAsync(response);");
                code.AppendLine("               return result;");
                code.AppendLine();

                code.AppendLine("           }");
                code.AppendLine("       }");

                op.CodeNamespace = this.CodeNamespace;

                var requestSource = (op.HttpMethod + "_" + op.Name).ToCsharpIdentitfier() + "Request.cs";
                if (!sources.ContainsKey(requestSource))
                    sources.Add(requestSource, op.GenerateRequestCode());

                var responseSource = (op.HttpMethod + "_" + op.Name).ToCsharpIdentitfier() + "Response.cs";
                if (!sources.ContainsKey(responseSource))
                    sources.Add(responseSource, op.GenerateResponseCode());
            }

            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace
            sources.Add(this.Name + ".cs", code.ToString());

            return Task.FromResult(sources);
        }

        protected override Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync()
        {
            return Task.FromResult(default(Tuple<string, string>));
        }

        protected override Task<Tuple<string, string>> GeneratePagingSourceCodeAsync()
        {
            return Task.FromResult(default(Tuple<string, string>));
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new NotImplementedException();
        }

        public override string OdataTranslator
        {
            get { return null; }
        }


        public Task OpenAsync()
        {
            var jo = JObject.Parse(File.ReadAllText(this.Har));
            var entries = jo.SelectTokens("$.log.entries").SelectMany(x => x);
            var operations = from j in entries
                             select new HttpOperationDefinition(j)
                             {
                                 Url = j.SelectToken("request.url").Value<string>(),
                                 HttpMethod = j.SelectToken("request.method").Value<string>(),
                                 Uuid = Guid.NewGuid().ToString()
                             };

            this.OperationDefinitionCollection.AddRange(operations);
            return Task.FromResult(0);
        }
    }
}
