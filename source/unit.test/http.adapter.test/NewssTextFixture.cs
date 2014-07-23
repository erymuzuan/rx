using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class NewssTextFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }

        public NewssTextFixture()
        {

            this.Schema = "UnitTest";
            this.Adapter = new HttpAdapter
            {
                Name = "NewssTextFixture",
                Schema = this.Schema,
                Har = @".\newss.har",
                Tables = new AdapterTable[] { },
                BaseAddress = "http://sit.stats.gov.my:7002/"
            };
        }

        private async Task OpenAsync()
        {
            await this.Adapter.OpenAsync();
            var count = 1;
            foreach (var op in this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace("http://sit.stats.gov.my:7002/", "")
                    .Replace("%20", "_")
                    .Replace(" ", "_")
                    .Replace("/", "_")
                    .Replace("?", "_")
                    .Replace("=", "_")
                    .Replace(".", "_") + "_" + count++;
                Console.WriteLine("METHOD => " + op.Name);
                op.IsLoginOperation = op.Name.ToLower().Contains("eplogin_seam") && op.HttpMethod == "POST";
                op.IsLoginRequired = op.HttpMethod == "POST" && !op.Name.ToLower().Contains("eplogin_seam");
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
        public async Task PopulatePostResponse()
        {
            await this.OpenAsync();
            var pdrm =
                this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .First(a => a.Name == "rilek_pdrm" && a.HttpMethod == "POST");

            var biodata = new RegexMember { Type = typeof(object), Name = "Biodata" };
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
                Name = "District",
                Type = typeof(string),
                Group = "district",
                Pattern = @"</td> <td>(?<date>\d\d [A-Za-z]{3} \d\d\d\d \d\d:\d\d:\d\d)</td> <td>(?<district>.*?)</td>"
            });
            summon.MemberCollection.Add(new RegexMember { Name = "Location", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "Offence", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "OrginalAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "CurrentAmount", Type = typeof(string) });
            summon.MemberCollection.Add(new RegexMember { Name = "EnforcementDate", Type = typeof(string) });
            pdrm.ResponseMemberCollection.Add(summon);

            var dll = Assembly.LoadFile(await CompileAsync());
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
            Assert.AreEqual("CHE ESHAH BINTI MAHMOOD", response.Biodata.FullName);
            Assert.AreEqual(1, response.SummonCollection.Count);
            Assert.AreEqual("DAL3429", response.SummonCollection[0].VehicleNo);

            Assert.AreEqual(70.00m, response.TotalAmount);
            Assert.AreEqual(DateTime.Today, response.DateTime.Date);

            Assert.AreEqual("KUALA LIPIS", response.SummonCollection[0].District);
            Assert.AreEqual(DateTime.Parse("2006-08-13 11:58:00"), response.SummonCollection[0].OffenceDate);

        }

        [TestMethod]
        public async Task PostMethodRequest()
        {
            var dll = Assembly.LoadFile(await CompileAsync());
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
    }
}
