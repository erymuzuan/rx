using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Humanizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class GetRequestRoutingTestFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }

        public GetRequestRoutingTestFixture()
        {

            this.Schema = "UnitTest";
            this.Adapter = new HttpAdapter
            {
                Name = "__WebApiAspNet",
                Schema = this.Schema,
                Har = @".\web-api-asp.net.har",
                Tables = new AdapterTable[] { },
                BaseAddress = "http://www.asp.net"
            };
        }

        private async Task OpenAsync()
        {
            await this.Adapter.OpenAsync();
            foreach (var op in this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace("http://www.asp.net.com.my/", "")
                    .Replace("%20", "_")
                    .Replace(":", "_")
                    .Replace(",", "_")
                    .Replace(" ", "_")
                    .Replace("-", "_")
                    .Replace("/", "_")
                    .Replace(".", "_");
            }

        }
        private async Task<string> CompileAsync()
        {
            if (this.Adapter.OperationDefinitionCollection.Count == 0)
                await this.OpenAsync();
            var result = await this.Adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));
            return result.Output;
        }

        [TestMethod]
        public async Task GetMethod()
        {
            await OpenAsync();
            var articleOp =
                this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .First(x => x.Url.Contains("/web-api/overview"));
            articleOp.RequestRouting = "/web-api/overview/{category}/{title}";
            articleOp.Name = "Web_Api_Article";
            articleOp.MethodName = "Web_Api_Article";
            articleOp.RequestMemberCollection.Add(new Member{Name = "title", Type = typeof(string)});
            articleOp.RequestMemberCollection.Add(new Member { Name = "category", Type = typeof(string) });


            var dll = Assembly.LoadFile(await this.CompileAsync());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType(string.Format("Dev.Adapters.{0}.GetWebApiArticleRequest", Adapter.Schema));
            dynamic aspnet = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);
            request.category = "working-with-http";
            request.title = "sending-html-form-data,-part-2";

            
            var response = await aspnet.GetWebApiArticleAsync(request);
            StringAssert.Contains(response.ResponseText, "Sending HTML Form Data");
        }
    }
}
