using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class ParseHtmlResponseTestFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }

        private async Task<string> CompileRilekHar()
        {
            this.Schema = "UnitTest";
            this.Adapter = new HttpAdapter
            {
                Name = "RilekSamanPdrmAdapter",
                Schema = this.Schema,
                Har = @".\rilek2.har",
                Tables = new AdapterTable[] { },
                BaseAddress = "https://www.rilek.com.my/"
            };

            await this.Adapter.OpenAsync();
            foreach (var op in this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace("https://www.rilek.com.my/rilek/", "")
                    .Replace("%20", "_")
                    .Replace(" ", "_")
                    .Replace("/", "_")
                    .Replace(".", "_");
                op.IsLoginOperation = op.Name == "users_login";
                op.IsLoginRequired = op.HttpMethod == "POST" && op.Name != "users_login";
            }
            var result = await this.Adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));
            return result.Output;
        }

        [TestMethod]
        public async Task PostMultipartFormData()
        {
            const string TEXT = @"------WebKitFormBoundaryxi5TIGPLxLVlaaYs
Content-Disposition: form-data; name=""search_by""

0
------WebKitFormBoundaryxi5TIGPLxLVlaaYs
Content-Disposition: form-data; name=""id_no""

500222035278
------WebKitFormBoundaryxi5TIGPLxLVlaaYs
Content-Disposition: form-data; name=""vehicle_no""


------WebKitFormBoundaryxi5TIGPLxLVlaaYs
Content-Disposition: form-data; name=""search""

Check Summon
------WebKitFormBoundaryxi5TIGPLxLVlaaYs--
";

            var baseAddress = new Uri("https://www.rilek.com.my");
            const string URL = "rilek/rilek/pdrm";
            var cookieContainer = new CookieContainer();
            await LoginAsync(cookieContainer);
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, URL)
                {
                    Content = new StringContent(TEXT, Encoding.UTF8)
                };
                requestMessage.Content.Headers.Remove("Content-Type");
                requestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundaryxi5TIGPLxLVlaaYs");

                var response = await client.SendAsync(requestMessage);
                var content2 = response.Content as StreamContent;
                if (null == content2) throw new Exception("Fail to read from response");
                var html = await content2.ReadAsStringAsync();
                StringAssert.Contains(html, "CHE ESHAH");

            }
        }

        private async Task LoginAsync(CookieContainer cookieContainer)
        {
            var baseAddress = new Uri("https://www.rilek.com.my");
            const string URL = "rilek/users/login";
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, URL)
                {
                    Content = new StringContent("email=erymuzuan@gmail.com&password=Yk25inHw&btnLogin=Login", Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await client.SendAsync(requestMessage);
                var content2 = response.Content as StreamContent;
                if (null == content2) throw new Exception("Fail to read from response");
                var html = await content2.ReadAsStringAsync();
                StringAssert.Contains(html, "PDRM");

            }
        }

        [TestMethod]
        public async Task GetPostRequestWithMultipartEncoded()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var loginType = dll.GetType("Dev.Adapters.UnitTest.POSTusers_loginRequest");
            var requestType = dll.GetType("Dev.Adapters.UnitTest.POSTrilek_pdrmRequest");

            dynamic login = Activator.CreateInstance(loginType);
            login.email = "erymuzuan@gmail.com";
            login.password = "Yk25inHw";
            login.btnLogin = "Login";

            dynamic request = Activator.CreateInstance(requestType);
            request.search_by = "0";
            request.id_no = "500222035278";
            request.vehicle_no = "";
            request.search = "Check Summon";

            dynamic rilek = Activator.CreateInstance(type);
            rilek.LoginCredential = login;
            var response = await rilek.POSTrilek_pdrmAsync(request);
            StringAssert.Contains(response.ResponseText, "CHE ESHAH");

        }

        [TestMethod]
        public async Task PostMethodRequest()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest.POSTrilek_pdrmRequest");
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);
            request.inUserName = "test321";
            request.inPassword = "6555555";
            request.submitf = "Login";

            var response = await localhost.POSTrilek_pdrmAsync(request);
            StringAssert.Contains(response.ResponseText, "Invalid User Name");
        }
    }
}
