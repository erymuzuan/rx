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
    public class ParseHtmlResponseTestFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }

        public ParseHtmlResponseTestFixture()
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
        }

        private async Task OpenAsync()
        {
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

        }
        private async Task<string> CompileRilekHar()
        {
            if (this.Adapter.OperationDefinitionCollection.Count == 0)
                await this.OpenAsync();
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
        public async Task PopulatePostResponse()
        {
            await this.OpenAsync();
            var pdrm =
                this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .First(a => a.Name == "rilek_pdrm" && a.HttpMethod == "POST");
            pdrm.ResponseMemberCollection.Add(new RegexMember
            {
                Name = "DateTime",
                Type = typeof(DateTime),
                Group = "date",
                DateFormat = "dd MMM yyyy HH:mm:ss",
                Pattern = @"Total Amount \(RM\)</th> </tr> </thead> <tbody> <tr> <td>(?<date>[0-9]{1,2} [A-Za-z]{3} [0-9]{4} [0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2})</td>"
            });
            pdrm.ResponseMemberCollection.Add(new RegexMember { Name = "FullName", Type = typeof(string), Group = "FullName", Pattern = @"Total Amount \(RM\)</th> </tr> </thead> <tbody> <tr> <td>[0-9]{1,2} [A-Za-z]{3} [0-9]{4} [0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2}</td> <td>(?<FullName>.*?)</td> " });
            pdrm.ResponseMemberCollection.Add(new RegexMember { Name = "MyKad", Type = typeof(string), Group = "id", Pattern = "<td>[0-9]{12}</td>" });
            pdrm.ResponseMemberCollection.Add(new RegexMember { Name = "Count", Type = typeof(int), Group = "c", Pattern = "<td>[0-9]{12}</td> <td style=\"text-align:center;\">(?<c>[0-9]{1,})</td>" });
            pdrm.ResponseMemberCollection.Add(new RegexMember
            {
                Name = "TotalAmount",
                IsNullable = true,
                Type = typeof(decimal),
                Group = "t",
                Pattern = "<td>[0-9]{12}</td> <td style=\"text-align:center;\">[0-9]{1,}</td> " +
                "<td style=\"text-align:center; font-weight:bold;\">(?<t>[0-9.]{1,})</td> </tr>"
            });

            var summon = new RegexMember { Name = "SummonCollection", Type = typeof(Array) };
            summon.MemberCollection.Add(new RegexMember { Name = "No", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "VehicleNo", Type = typeof(string), Group = "plate", Pattern = "[0-9]{2}[A-Z]{2}[0-9]{6}</td> <td>(?<plate>.*?)</td> <td>[0-9]{1,2} [A-Za-z]{3} [0-9]{4}" });
            summon.MemberCollection.Add(new RegexMember { Name = "OffenceDate", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "District", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "Location", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "Offence", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "OrginalAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "CurrentAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "EnforcementDate", Type = typeof(string) });
            pdrm.ResponseMemberCollection.Add(summon);

            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var loginType = dll.GetType("Dev.Adapters.UnitTest.PostUsersLoginRequest");
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostRilekPdrmRequest");

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
            var response = await rilek.PostRilekPdrmAsync(request);
            StringAssert.Contains(response.ResponseText, "CHE ESHAH");
            Assert.AreEqual("CHE ESHAH BINTI MAHMOOD", response.FullName);
            Assert.AreEqual(1, response.SummonCollection.Count);
            Assert.AreEqual("DAL3429", response.SummonCollection[0].VehicleNo);

            Assert.AreEqual(70.00m, response.TotalAmount);
            Assert.AreEqual(DateTime.Today, response.DateTime.Date);

        }
        [TestMethod]
        public async Task GetPostRequestWithMultipartEncoded()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var loginType = dll.GetType("Dev.Adapters.UnitTest.PostUsersLoginRequest");
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostRilekPdrmRequest");

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
            var response = await rilek.PostRilekPdrmAsync(request);
            StringAssert.Contains(response.ResponseText, "CHE ESHAH");

        }

        [TestMethod]
        public async Task PostMethodRequest()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostUsersLoginRequest");
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);
            request.email = "test321@tst.com";
            request.password = "6555555";
            request.btnLogin = "Login";

            var response = await localhost.PostUsersLoginAsync(request);
            StringAssert.Contains(response.ResponseText, "Incorrect Login");
        }
        [TestMethod]
        public void Dehumanize()
        {
            const string M = "POST_users_login_Request";
            Assert.AreEqual("PostUsersLoginRequest", M.ToLower().Humanize().Transform(To.TitleCase).Replace(" ", ""));
        }
        [TestMethod]
        public void DehumanizeToChasrp()
        {
            const string M = "POST_UsersLogin_Request";
            Assert.AreEqual("PostUsersLoginRequest", M.ToLower().Humanize().Transform(To.TitleCase).Replace(" ", ""));
        }
    }
}
