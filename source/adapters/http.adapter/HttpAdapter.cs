﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpAdapter : Adapter
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
            foreach (var op in this.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                var name = op.Name;
                var adapterName = name + "Adapter";
                if (sources.ContainsKey(adapterName + ".cs")) continue;
                code.AppendLinf("       public async Task<{1}{0}Response> {1}{2}Async({1}{0}Request request)", op.Name, op.HttpMethod, op.Name);
                code.AppendLine("       {");
                code.AppendLinf("           const string url = \"{0}\";", op.Url);
                code.AppendLine("           using(var client = new HttpClient())");
                code.AppendLine("           {");
                switch (op.HttpMethod)
                {
                    case "GET":
                        code.AppendLine("               var response = await client.GetAsync(url);");
                        break;
                    case "DELETE":
                        code.AppendLine("               var response = await client.DeleteAsync(url);");
                        break;
                    case "POST":
                        if (op.RequestHeaders.ContainsKey("Content-Type"))
                        {
                            code.AppendLine("               var requestMessage = new  HttpRequestMessage(HttpMethod.Post,url);");
                            code.AppendLinf("               requestMessage.Content = new StringContent(request.PostData, Encoding.UTF8, \"{0}\");", op.RequestHeaders["Content-Type"]);
                            code.AppendLine("               var response = await client.SendAsync(requestMessage);");
                            code.AppendLine("               ");

                        }
                        else
                        {

                            code.AppendLine("               var content = new StringContent(request.PostData);");
                            code.AppendLine("               var response = await client.PostAsync(url, content);");
                        }
                        break;
                    case "PUT":
                        code.AppendLine("               var content = new StringContent(\"TODO\");");
                        code.AppendLine("               var response = await client.PutAsync(url, content);");
                        break;
                    case "PATCH":
                        code.AppendLine("               var content = new StringContent(\"TODO\");");
                        code.AppendLine("               var response = await client.PatchAsync(url, content);");
                        break;
                }
                code.AppendLinf("               var result =  new {0}{1}Response();", op.HttpMethod, op.Name);
                code.AppendLine("               await result.LoadAsync(response);");
                code.AppendLine("               return result;");
                code.AppendLine();

                code.AppendLine("           }");
                code.AppendLine("       }");

                op.CodeNamespace = this.CodeNamespace;
                sources.Add(op.HttpMethod + op.Name + "Request.cs", op.GenerateRequestCode());
                sources.Add(op.HttpMethod + op.Name + "Response.cs", op.GenerateResponseCode());
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
            get { throw new NotImplementedException(); }
        }

        public string Har { get; set; }

        public Task OpenAsync()
        {
            var jo = JObject.Parse(File.ReadAllText(this.Har));
            var entries = jo.SelectTokens("$.log.entries").SelectMany(x => x);
            var operations = from j in entries
                             select new HttpOperationDefinition(j)
                             {
                                 Url = j.SelectToken("request.url").Value<string>(),
                                 HttpMethod = j.SelectToken("request.method").Value<string>()

                             };

            this.OperationDefinitionCollection.AddRange(operations);
            return Task.FromResult(0);
        }
    }
}
