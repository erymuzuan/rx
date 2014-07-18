using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class HttpAdapterTestFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }
        private async Task<string> CompilePetronasSapHar()
        {
            this.Schema = "UnitTest";
            this.Adapter = new HttpAdapter
            {
                Name = "HttpLocalhostTestAdapter",
                Schema = this.Schema,
                Har = @".\localhost.har",
                Tables = new AdapterTable[] { }
            };

            await this.Adapter.OpenAsync();
            foreach (var op in this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace("https://eservice.mystation.com.my/myicss/", "")
                    .Replace(".", "_");
            }
            var result = await this.Adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));
            return result.Output;
        }

        [TestMethod]
        public async Task GetMethod()
        {
            var dll = Assembly.LoadFile(await CompilePetronasSapHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType(string.Format("Dev.Adapters.{0}.GETlogin_doRequest", Adapter.Schema));
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);

            var response = await localhost.GETlogin_doAsync(request);
            StringAssert.Contains(response.ResponseText, "inUserName");
        }

        [TestMethod]
        public async Task PostMethodRequestClient()
        {
            const string URL = "https://eservice.mystation.com.my/myicss/login.do";
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post,URL)
                {
                    Content =
                        new StringContent("inUserName=test321&inPassword=6555555&submitf=Login", Encoding.UTF8,
                            "application/x-www-form-urlencoded")
                };
                ;
                var response = await client.SendAsync(requestMessage);

                var content2 = response.Content as StreamContent;
                if (null == content2) throw new Exception("Fail to read from response");
                var r = await content2.ReadAsStringAsync();
                StringAssert.Contains(r, "Invalid User Name");
            }
        }
        [TestMethod]
        public async Task PostMethodRequest()
        {
            var dll = Assembly.LoadFile(await CompilePetronasSapHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType(string.Format("Dev.Adapters.{0}.POSTlogin_doRequest", Adapter.Schema));
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);
            request.inUserName = "test321";
            request.inPassword = "6555555";
            request.submitf = "Login";

            var response = await localhost.POSTlogin_doAsync(request);
            StringAssert.Contains(response.ResponseText, "Invalid User Name");
        }
    }
}
