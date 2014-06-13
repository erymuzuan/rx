using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
        //[]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [TestMethod]
        public async Task GenerateMetadataAsync()
        {
            var tables = new[]
            {
                new AdapterTable{Name = "Person"},
                new AdapterTable{Name = "Address"}
            };
            var sql = new SqlServerAdapter
            {
                Server = @"(localdb)\ProjectsV12",
                Database = "AdventureWorks",
                TrustedConnection = true,
                Schema = "Person",
                Tables = tables,
                Name = "AdventureWorksPersons",
                Description = "A test"

            };
            await sql.OpenAsync();

            var result = await sql.CompileAsync();
            var dll = Assembly.LoadFile(result.Output);
            var personType = dll.GetType("Dev.Adapters.Person.Person");
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
            prs.BusinessEntityID = sql.ConnectionString.GetDatabaseScalarValue<int>("SELECT MAX(BusinessEntityID) FROM Person.BusinessEntity") + 1;

            sql.ConnectionString.ExecuteNonQuery("INSERT INTO Person.BusinessEntity(rowguid, ModifiedDate) VALUES(@rowguid, @ModifiedDate)",
                new SqlParameter("@rowguid", prs.rowguid),
                new SqlParameter("@ModifiedDate", DateTime.Now));


            var adapterType = personType.Assembly.GetType("Dev.Adapters.Person.PersonAdapter");
            Assert.IsNotNull(adapterType);
            dynamic adapter = Activator.CreateInstance(adapterType);

            // delete

            await adapter.InsertAsync(prs);

            var person2 = await adapter.LoadOneAsync(prs.BusinessEntityID);
            Assert.IsNotNull(person2);
            Assert.AreEqual(prs.FirstName, person2.FirstName);
            await adapter.DeleteAsync(prs.BusinessEntityID);

            var loadAfterDeleted = await adapter.LoadOneAsync(prs.BusinessEntityID);
            Assert.IsNull(loadAfterDeleted);


            // now test the API
            File.Copy(result.Output, ConfigurationManager.WebPath + @"\bin\" + Path.GetFileName(result.Output),true);
            Console.WriteLine("copying files");
            await Task.Delay(1000);
            const string URL = "api/person/person?filter=Title eq 'Ms.'&includeTotal=true&page=2&size=5";

            string json;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");

                var response = await client.GetStringAsync(URL);
                json = response;

            }
            var msCount = sql.ConnectionString.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Person].[Person] WHERE [Title] = 'Ms.'");
            Assert.IsNotNull(json);
            var lo = JObject.Parse(json);
            Assert.AreEqual(msCount, lo.SelectToken("$.count").Value<int>(), "count shoud be " + msCount);

            // call api to insert


            prs.FirstName = Guid.NewGuid().ToString().Substring(0, 8);
            prs.rowguid = Guid.NewGuid();

            sql.ConnectionString.ExecuteNonQuery("INSERT INTO Person.BusinessEntity(rowguid, ModifiedDate) VALUES(@rowguid, @ModifiedDate)",
                new SqlParameter("@rowguid", prs.rowguid),
                new SqlParameter("@ModifiedDate", DateTime.Now));
            prs.BusinessEntityID = sql.ConnectionString.GetDatabaseScalarValue<int>("SELECT [BusinessEntityID] FROM Person.BusinessEntity WHERE [rowguid] = @guid",
                new SqlParameter("@guid", prs.rowguid)) ; 

            // HTTP API - insert
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var content = new StringContent(JsonSerializerService.ToJsonString(prs,true));
                var response = await client.PostAsync(URL, content);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var prsCount = sql.ConnectionString.GetDatabaseScalarValue<int>(
                    "SELECT [BusinessEntityId] FROM [Person].[Person] WHERE [FirstName] = @FirstName", 
                    new SqlParameter("@FirstName",prs.FirstName));
                Assert.AreEqual(prs.BusinessEntityID,prsCount);
            }

            // HTTP API - update
            prs.LastName = "muhammad";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var content = new StringContent(JsonSerializerService.ToJsonString(prs,true));
                var response = await client.PutAsync(URL, content);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var lastName = sql.ConnectionString.GetDatabaseScalarValue<string>(
                    "SELECT [LastName] FROM [Person].[Person] WHERE [BusinessEntityId] = @id", 
                    new SqlParameter("@id",prs.BusinessEntityID));
                Assert.AreEqual("muhammad", lastName);
            }

            // HTTP API - delete
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4436");
                var response = await client.DeleteAsync("api/person/person/" + prs.BusinessEntityID);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
                var deleted = sql.ConnectionString.GetDatabaseScalarValue<int>(
                    "SELECT COUNT(*) FROM [Person].[Person] WHERE [BusinessEntityId] = @id", 
                    new SqlParameter("@id",prs.BusinessEntityID));
                Assert.AreEqual(0, deleted);
            }

        }
    }
}
