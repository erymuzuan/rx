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
                Name = "__RilekSamanPdrmAdapter",
                Schema = this.Schema,
                Har = @".\rilek2.har",
                Tables = new AdapterTable[] { },
                BaseAddress = "https://www.rilek.com.my/rilek/",
                Timeout = 5000,
                TimeoutInterval = "Minutes"
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
                    .Replace("rilek_rilek", "rilek")
                    .Replace(".", "_");
                op.MethodName = op.Name.ToCsharpIdentitfier();
                Console.WriteLine("{0}=>{1}", op.Name, op.MethodName);
                op.IsLoginOperation = op.MethodName == "RilekUsersLogin";
                op.IsLoginRequired = op.HttpMethod == "POST" && op.MethodName != "RilekUsersLogin";
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
                client.Timeout = TimeSpan.FromMinutes(5);
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
            var pdrm = this.Adapter.OperationDefinitionCollection
                            .OfType<HttpOperationDefinition>()
                            .First(a => a.Name == "_rilek_pdrm" && a.HttpMethod == "POST");

            var biodata = new RegexMember {Type = typeof (object), Name = "Biodata"};
            pdrm.ResponseMemberCollection.Add(biodata);
            pdrm.ResponseMemberCollection.Add(new RegexMember
            {
                Name = "DateTime",
                Type = typeof(DateTime),
                Group = "date",
                DateFormat = "dd MMM yyyy HH:mm:ss",
                Pattern = @"Total Amount \(RM\)</th> </tr> </thead> <tbody> <tr> <td>(?<date>[0-9]{1,2} [A-Za-z]{3} [0-9]{4} [0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2})</td>"
            });
            biodata.MemberCollection.Add(new RegexMember
            {
                Name = "FullName",
                Type = typeof(string),
                Group = "FullName",
                Pattern = @"Total Amount \(RM\)</th> </tr> </thead> <tbody> <tr> <td>[0-9]{1,2} [A-Za-z]{3} [0-9]{4} [0-9]{1,2}:[0-9]{1,2}:[0-9]{1,2}</td> <td>(?<FullName>.*?)</td> "
            });
            biodata.MemberCollection.Add(new RegexMember
            {
                Name = "MyKad",
                Type = typeof(string),
                Group = "id",
                Pattern = "<td>[0-9]{12}</td>"
            });
            pdrm.ResponseMemberCollection.Add(new RegexMember
            {
                Name = "Count",
                Type = typeof(int),
                Group = "c",
                Pattern = "<td>[0-9]{12}</td> <td style=\"text-align:center;\">(?<c>[0-9]{1,})</td>"
            });
            pdrm.ResponseMemberCollection.Add(new RegexMember
            {
                Name = "TotalAmount",
                IsNullable = true,
                Type = typeof(decimal),
                Group = "total",
                Pattern = "<td>[0-9]{12}</td> <td style=\"text-align:center;\">[0-9]{1,}</td> " +
                "<td style=\"text-align:center; font-weight:bold;\">(?<total>[0-9.]{1,})</td> </tr>"
            });

            var summon = new RegexMember { Name = "SummonCollection", Type = typeof(Array) };
            summon.MemberCollection.Add(new RegexMember { Name = "No", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember
            {
                Name = "VehicleNo",
                Type = typeof(string),
                Group = "plate",
                Pattern = "[0-9]{2}[A-Z]{2}[0-9]{6}</td> <td>(?<plate>.*?)</td> <td>[0-9]{1,2} [A-Za-z]{3} [0-9]{4}"
            });
            summon.MemberCollection.Add(new RegexMember
            {
                Name = "OffenceDate",
                Type = typeof(DateTime),
                Group = "date",
                Pattern = @"</td> <td>(?<date>\d\d [A-Za-z]{3} \d\d\d\d \d\d:\d\d:\d\d)</td>",
                DateFormat = "dd MMM yyyy HH:mm:ss"
            });
            summon.MemberCollection.Add(new RegexMember
            {
                Name = "District", Type = typeof(string),
                Group = "district",
                Pattern = @"</td> <td>(?<date>\d\d [A-Za-z]{3} \d\d\d\d \d\d:\d\d:\d\d)</td> <td>(?<district>.*?)</td>"
            });
            summon.MemberCollection.Add(new RegexMember { Name = "Location", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "Offence", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "OrginalAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "CurrentAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "EnforcementDate", Type = typeof(string) });
            pdrm.ResponseMemberCollection.Add(summon);

            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var loginType = dll.GetType("Dev.Adapters.UnitTest.PostRilekUsersLoginRequest");
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
            var authenticated = await rilek.PostRilekUsersLoginAsync(login);
            Console.WriteLine(JsonSerializerService.ToJsonString(authenticated, true));

            var response = await rilek.PostRilekPdrmAsync(request);
            rilek.Dispose();

            StringAssert.Contains(response.ResponseText, "CHE ESHAH");
            Assert.AreEqual("CHE ESHAH BINTI MAHMOOD", response.Biodata.FullName);
            Assert.AreEqual(1, response.SummonCollection.Count);
            Assert.AreEqual("DAL3429", response.SummonCollection[0].VehicleNo);

            Assert.AreEqual(70.00m, response.TotalAmount);
            Assert.AreEqual(DateTime.Today, response.DateTime.Date);

            Assert.AreEqual("KUALA LIPIS", response.SummonCollection[0].District);
            Assert.AreEqual(DateTime.Parse("2006-08-13 11:58:00"), response.SummonCollection[0].OffenceDate);

        }
        [TestMethod]
        public async Task PostRequestWithMultipartForDataEncoded()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var loginType = dll.GetType("Dev.Adapters.UnitTest.PostRilekUsersLoginRequest");
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

            await rilek.PostRilekUsersLoginAsync(login);
            var response = await rilek.PostRilekPdrmAsync(request);
            StringAssert.Contains(response.ResponseText, "CHE ESHAH");



        }

        [TestMethod]
        public async Task PostMethodRequest()
        {
            var dll = Assembly.LoadFile(await CompileRilekHar());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostRilekUsersLoginRequest");
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);
            request.email = "test321@tst.com";
            request.password = "6555555";
            request.btnLogin = "Login";

            var response = await localhost.PostRilekUsersLoginAsync(request);
            StringAssert.Contains(response.ResponseText, "Incorrect Login");
        }
     
    }
}
