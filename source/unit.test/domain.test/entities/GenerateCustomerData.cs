using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amido.NAuto;
using Amido.NAuto.Randomizers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class GenerateCustomerData
    {
        [Test]
        public async Task GenerateLotsOfRows()
        {
            var xml = "sph".GetDatabaseScalarValue<string>("SELECT [Data] FROM [Sph].[EntityDefinition] WHERE [Name] = 'Customer'");
            var ed = XElement.Parse(xml).Deserialize<EntityDefinition>();
            Assert.IsNotNull(ed);

            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };


            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll")));

            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            var assembly = Assembly.LoadFrom(result.Output);
            var type = assembly.GetType("Bespoke.Dev_2.Domain.Customer");

            "sph".ExecuteNonQuery("TRUNCATE TABLE [Dev].[Customer]");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync("dev/customer");
                Console.WriteLine(response.StatusCode);
            }
            for (int i = 0; i < 100; i++)
            {
                dynamic customer = Activator.CreateInstance(type);
                customer.FullName = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);
                customer.Age = NAuto.GetRandomInteger(25, 65);
                customer.Gender = new[] { "Male", "Female" }.OrderBy(f => Guid.NewGuid()).First();
                customer.Address.State = new[] { "Kelantan", "Selangor", "Perak", "Kuala Lumpur", "Johor", "Melaka", "Negeri Sembilan" }.OrderBy(f => Guid.NewGuid()).First();
                customer.Address.Locality = new[] { "Rural", "Urban", "Surburb" }.OrderBy(f => Guid.NewGuid()).First();
                customer.RegisteredDate = DateTime.Today.AddDays(-NAuto.GetRandomInteger(50, 500));

                customer.IsPriority = customer.FullName.Length % 2 == 0;
                customer.Contact.Name = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);
                customer.Address.Street1 = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);

                string sql = string.Format(@"INSERT INTO [dev].[Customer]
           ([Age]
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
           ({0}
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

                    ", customer.Age, customer.Gender, customer.IsPriority ? "1" : "0",
                     customer.RegisteredDate
                     , customer.Address.State
                     , customer.Address.Locality
                     , customer.Contact.Name
                     );
                Console.WriteLine(i);
                var jsonP = new SqlParameter("@Json", JsonSerializerService.ToJsonString(customer));

                "sph".ExecuteNonQuery(sql, jsonP);

                // es
                var setting = new JsonSerializerSettings();
                var json = JsonConvert.SerializeObject(customer, setting);

                var content = new StringContent(json);
                var id = (i + 1);


                var url = string.Format("/dev/customer/{0}", id);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                    var response = await client.PutAsync(url, content);
                    Console.WriteLine(response.StatusCode);
                }


            }

        }
    }
}
