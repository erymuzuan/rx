using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace sqlserver.adapter.test
{
    [TestClass]
    public class SqlServerAdapterTest
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Console.WriteLine("init................");
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            var tables = new[]
            {
                new AdapterTable{Name = "Person"},
                new AdapterTable{Name = "Address"}
            };
            m_sql = new SqlServerAdapter
            {
                Server = @"(localdb)\ProjectsV12",
                Database = "AdventureWorks",
                TrustedConnection = true,
                Schema = "Person",
                Tables = tables,
                Name = "AdventureWorksPersons",
                Description = "A test"

            };

            var bin = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.dll";
            if (File.Exists(bin))
                File.Delete(bin);
            var pdb = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.pdb";
            if (File.Exists(pdb))
                File.Delete(pdb);
        }

        private static SqlServerAdapter m_sql;
        private static Assembly m_dll;
        public async Task CompileAsync()
        {
            var bin = ConfigurationManager.WebPath + @"\bin\Dev.AdventureWorksPersons.dll";
            if (File.Exists(bin))
            {
                m_dll = Assembly.LoadFile(bin);
                return;
            }

            await m_sql.OpenAsync();

            var result = await m_sql.CompileAsync();
            m_dll = Assembly.LoadFile(result.Output);
            File.Copy(result.Output, bin, true);
            Console.WriteLine("copying files");
            await Task.Delay(1000);

        }


        private dynamic CreatePerson()
        {
            var personType = m_dll.GetType("Dev.Adapters.Person.Person");
            dynamic prs = Activator.CreateInstance(personType);
            Assert.IsNotNull(prs);
            prs.PersonType = "EM";
            prs.FirstName = Guid.NewGuid().ToString().Substring(0, 8);
            prs.LastName = "mustapa";
            prs.MiddleName = "bin";
            prs.Title = "Prog";
            prs.NameStyle = true;
            prs.ModifiedDate = DateTime.Now;
            prs.rowguid = Guid.NewGuid();
            prs.EmailPromotion = 1;
            prs.Suffix = "Mr";
            prs.BusinessEntityID = m_sql.GetDatabaseScalarValue<int>("SELECT MAX(BusinessEntityID) FROM Person.BusinessEntity") + 1;
            return prs;
        }
        [TestMethod]
        public async Task AdapterInsert()
        {
            await this.CompileAsync();
            var prs = this.CreatePerson();
            m_sql.ExecuteNonQuery("INSERT INTO Person.BusinessEntity(rowguid, ModifiedDate) VALUES(@rowguid, @ModifiedDate)",
                new SqlParameter("@rowguid", prs.rowguid),
                new SqlParameter("@ModifiedDate", DateTime.Now));



            // delete
            var adapter = this.GetAdapter();
            await adapter.InsertAsync(prs);
        }

        private dynamic GetAdapter()
        {
            var adapterType = m_dll.GetType("Dev.Adapters.Person.PersonAdapter");
            Assert.IsNotNull(adapterType);
            dynamic adapter = Activator.CreateInstance(adapterType);
            return adapter;
        }

        [TestMethod]
        public async Task AdapterLoadOne()
        {
            await this.CompileAsync();
            var adapter = this.GetAdapter();
            var id = m_sql.GetDatabaseScalarValue<int>(
                 "SELECT TOP 1 [BusinessEntityID] FROM [Person].[Person] ORDER BY NEWID()");
            var firstName = m_sql.GetDatabaseScalarValue<string>(
                 "SELECT [FirstName] FROM [Person].[Person] WHERE [BusinessEntityID] = @BusinessEntityID",
                 new SqlParameter("@BusinessEntityID", id));

            var person2 = await adapter.LoadOneAsync(id);
            Assert.IsNotNull(person2);
            Assert.AreEqual(firstName, person2.FirstName);


        }
        [TestMethod]
        public async Task AdapterDelete()
        {
            await this.CompileAsync();
            var adapter = this.GetAdapter();
            var id = m_sql.GetDatabaseScalarValue<int>(
                 "SELECT MAX([BusinessEntityID]) FROM [Person].[Person]");

            await adapter.DeleteAsync(id);
            var loadAfterDeleted = await adapter.LoadOneAsync(id);
            Assert.IsNull(loadAfterDeleted);


        }

        [TestMethod]
        public async Task HttpPostInsert()
        {
            await this.CompileAsync();
            dynamic prs = this.CreatePerson();

            prs.FirstName = Guid.NewGuid().ToString().Substring(0, 8);
            prs.rowguid = Guid.NewGuid();

            m_sql.ExecuteNonQuery("INSERT INTO Person.BusinessEntity(rowguid, ModifiedDate) VALUES(@rowguid, @ModifiedDate)",
                new SqlParameter("@rowguid", prs.rowguid),
                new SqlParameter("@ModifiedDate", DateTime.Now));
            prs.BusinessEntityID = m_sql.GetDatabaseScalarValue<int>("SELECT [BusinessEntityID] FROM Person.BusinessEntity WHERE [rowguid] = @guid",
                new SqlParameter("@guid", prs.rowguid));

            // HTTP API - insert
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var content = new StringContent(JsonSerializerService.ToJsonString(prs, true), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/person/person", content);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var prsCount = m_sql.GetDatabaseScalarValue<int>(
                    "SELECT [BusinessEntityId] FROM [Person].[Person] WHERE [FirstName] = @FirstName",
                    new SqlParameter("@FirstName", prs.FirstName));
                Assert.AreEqual(prs.BusinessEntityID, prsCount);
            }
        }

        [TestMethod]
        public async Task HttpPutUpdate()
        {

            await this.CompileAsync();
            dynamic prs = this.CreatePerson();
            prs.BusinessEntityID = m_sql.GetDatabaseScalarValue<int>("SELECT MAX(BusinessEntityID) FROM [Person].[Person]");
            // HTTP API - update
            prs.LastName = "muhammad";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var content = new StringContent(JsonSerializerService.ToJsonString(prs, true), Encoding.UTF8, "application/json");
                var response = await client.PutAsync("/api/person/person", content);
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
            }

            var lastName = m_sql.GetDatabaseScalarValue<string>(
                "SELECT [LastName] FROM [Person].[Person] WHERE [BusinessEntityId] = @id",
                new SqlParameter("@id", prs.BusinessEntityID));
            Assert.AreEqual("muhammad", lastName);
        }

        [TestMethod]
        public async Task HttpDeleteRemove()
        {
            await this.CompileAsync();
            var id = m_sql.GetDatabaseScalarValue<int>(
                 "SELECT MAX([BusinessEntityID]) FROM [Person].[Person]");

            // HTTP API - delete
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var response = await client.DeleteAsync("api/person/person/" + id);


                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var deleted = m_sql.GetDatabaseScalarValue<int>(
                    "SELECT COUNT(*) FROM [Person].[Person] WHERE [BusinessEntityId] = @id",
                    new SqlParameter("@id", id));
                Assert.AreEqual(0, deleted);
            }
        }



        [TestMethod]
        private async Task<string> HttpGetList()
        {
            await this.CompileAsync();
            // now test the API
            const string URL = "api/person/person?filter=Title eq 'Ms.'&includeTotal=true&page=2&size=5";

            string json;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");

                var response = await client.GetStringAsync(URL);
                json = response;
            }
            var msCount =
                m_sql.GetDatabaseScalarValue<int>(
                    "SELECT COUNT(*) FROM [Person].[Person] WHERE [Title] = 'Ms.'");
            Assert.IsNotNull(json);
            var lo = JObject.Parse(json);
            Assert.AreEqual(msCount, lo.SelectToken("$.count").Value<int>(), "count shoud be " + msCount);
            return URL;
        }
    }
}
