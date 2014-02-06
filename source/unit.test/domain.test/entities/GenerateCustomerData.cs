using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using NUnit.Framework;
using subscriber.entities;

namespace domain.test.entities
{
    [TestFixture]
    public class GenerateCustomerData
    {
        private const string SphConnection = "sph";

        [Test]
        public async Task RestoreAllCustomerView()
        {
            var vd = File.ReadAllText(Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityView/All customers.json"));
            var ed = vd.DeserializeFromJson<EntityView>();
            Assert.IsNotNull(ed);

            await this.InsertEntityViewAsync(ed);
        }
        [Test]
        public async Task RestoreAllCustomerForms()
        {
            var vd = File.ReadAllText(Path.Combine(ConfigurationManager.WorkflowSourceDirectory, @"EntityForm\Add New Customer.json"));
            var ed = vd.DeserializeFromJson<EntityForm>();
            Assert.IsNotNull(ed);

            await this.InsertEntityFormAsync(ed);
        }
        [Test]
        public async Task GenerateLotsOfRows()
        {

            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityDefinition/Customer.json"));
            var ed = customerDefinition.DeserializeFromJson<EntityDefinition>();
            Assert.IsNotNull(ed);

            await InsertCustomerDefinitionIntoSql(ed);
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);

            var sqlSub = new SqlTableSubscriber();
            await sqlSub.ProcessMessageAsync(ed);
            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Dev].[Customer]");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync("dev");
                Console.WriteLine("DELETE dev : {0}", response.StatusCode);

                await client.PutAsync("dev", new StringContent(""));
                // mapping
                var subs = new EntityIndexerMappingSubscriber();
                await subs.ProcessMessageAsync(ed);
            }
            for (int i = 0; i < 10000; i++)
            {
                Console.Write(".");
                var customer = CustomerEntityHelper.CreateCustomerInstance(type);
                customer.CustomerId = (i + 1);

                var sql = InsertCustomerInstanceIntoSqlAsync(customer);
                var es = InsertCustomerInstanceIntoElasticSearch(customer, i);
                await Task.WhenAll(sql, es);
            }

        }

        private static async Task InsertCustomerInstanceIntoElasticSearch(dynamic customer, int i)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(customer, setting);

            var content = new StringContent(json);
            var id = (i + 1);


            var url = string.Format("/dev/customer/{0}", id);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PutAsync(url, content);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
        }

        private async Task InsertCustomerInstanceIntoSqlAsync(dynamic customer)
        {
            string sql = string.Format(@"
SET IDENTITY_INSERT [dev].[Customer] ON
INSERT INTO [dev].[Customer]
           (
           [CustomerId]
           ,[Age]
           ,[Gender]
           ,[IsPriority]
           ,[RegisteredDate]
           ,[Address.State]
           ,[Address.Locality]
           ,[Contact.Name]
           ,[Json]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           ({7}
           ,{0}
           ,'{1}'
           ,{2}
           ,'{3:s}'
           ,'{4}'
           ,'{5}'
           ,'{6}'
           ,@Json
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')

              
SET IDENTITY_INSERT [dev].[Customer] OFF      ", customer.Age, customer.Gender, customer.IsPriority ? "1" : "0",
                customer.RegisteredDate
                , customer.Address.State
                , customer.Address.Locality
                , customer.Contact.Name
                , customer.CustomerId
                );

            var jsonP = new SqlParameter("@Json", JsonSerializerService.ToJsonString(customer));

            await SphConnection.ExecuteNonQueryAsync(sql, jsonP);


        }

     
        private async Task InsertEntityFormAsync(EntityForm form)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityForm]");
            var sql = string.Format(@"INSERT INTO [Sph].[EntityForm]
           ([Data]
           ,[Name]
           ,[Route]
           ,[EntityDefinitionId]
           ,[IsDefault]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,{2}
           ,{3}
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", form.Name, form.Route, form.EntityDefinitionId, form.IsDefault ? "1": "0");
            await SphConnection.ExecuteNonQueryAsync(sql, new SqlParameter("@xml", form.ToXmlString()));
        }

        private async Task InsertEntityViewAsync(EntityView view)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityView]");
            var sql = string.Format(@"INSERT INTO [Sph].[EntityView]
           ([Data]
           ,[Name]
           ,[Route]
           ,[EntityDefinitionId]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,{2}
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", view.Name, view.Route, view.EntityDefinitionId);
            await SphConnection.ExecuteNonQueryAsync(sql, new SqlParameter("@xml", view.ToXmlString()));
        }

        private async Task InsertCustomerDefinitionIntoSql(EntityDefinition ed)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityDefinition]");
            var insertCustomerDefinitionSql = string.Format(@"INSERT INTO [Sph].[EntityDefinition]
           ([Data]
           ,[Name]
           ,[Plural]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", ed.Name, ed.Plural);
            await SphConnection.ExecuteNonQueryAsync(insertCustomerDefinitionSql, new SqlParameter("@xml", ed.ToXmlString()));
        }
    }
}
