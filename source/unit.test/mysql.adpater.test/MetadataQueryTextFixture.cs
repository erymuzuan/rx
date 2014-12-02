using System;
using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mysql.adpater.test
{
    [TestClass]
    public class MetadataQueryTextFixture
    {
        const string ConnectionString = "Server=localhost;Database=employees;Uid=root;Pwd=;";
        [TestMethod]
        public async Task QueryDatabases()
        {
            var adapter = new MySqlAdapter { ConnectionString = ConnectionString };
            var controller = new MySqlAdapterController();
            var response = await controller.GetDatabasesAsync(adapter);
            var content = (JsonContent)response.Content;

            Console.WriteLine(await content.ReadAsStringAsync());

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task QuerySchemas()
        {
            var adapter = new MySqlAdapter
            {
                ConnectionString = ConnectionString,
                Database = "employees"
            };
            var controller = new MySqlAdapterController();
            var response = await controller.GetSchemasAsync(adapter);
            var content = (JsonContent)response.Content;

            var json = await content.ReadAsStringAsync();
            StringAssert.Contains(json,"employees");
            Assert.IsNotNull(response);
        }


        [TestMethod]
        public async Task QueryTables()
        {
            var adapter = new MySqlAdapter
            {
                ConnectionString = ConnectionString,
                Schema = "employees",
                Database = "employees",
                Server = "localhost",
                UserId = "root",
                Password = ""
            };
            var controller = new MySqlAdapterController();
            var response = await controller.GetObjectsAsync(adapter);
            var content = (JsonContent)response.Content;

            var json = await content.ReadAsStringAsync();
            StringAssert.Contains(json, "titles");
        }

        [TestMethod]
        public async Task QueryProcedure()
        {
            var adapter = new MySqlAdapter
            {
                ConnectionString = ConnectionString,
                Schema = "employees",
                Database = "employees",
                Server = "localhost",
                UserId = "root",
                Password = ""
            };
            var controller = new MySqlAdapterController();
            var response = await controller.GetObjectsAsync(adapter);
            var content = (JsonContent)response.Content;

            var text = await content.ReadAsStringAsync();

            var json = JObject.Parse(text).ToString(Formatting.Indented);
            StringAssert.Contains(json, "getStaffCountByTitle");
            StringAssert.Contains(json, "@count");
            /*''
             * 
             * CREATE DEFINER=`root`@`localhost` PROCEDURE `getStaffCountByTitle`(IN title VARCHAR(255), OUT count INT)
BEGIN 
  
    SELECT COUNT(*) INTO count FROM employees.titles
      WHERE 'title' = title;
  
  END
             * */
        }
    }
}
