using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mysql.adpater.test
{
    [TestClass]
    public class MetadataQueryTextFixture
    {
        [TestMethod]
        public async Task QueryDatabases()
        {
            var adapter = new MySqlAdapter("employees") ;
            var controller = new MySqlAdapterController();
            var response = await controller.GetDatabasesAsync(adapter);
            var content = (JsonContent)response.Content;

            Console.WriteLine(await content.ReadAsStringAsync());

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task QuerySchemas()
        {
            var adapter = new MySqlAdapter("employees");
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
            var adapter = new MySqlAdapter("employees");
            var controller = new MySqlAdapterController();
            var response = await controller.GetObjectsAsync(adapter);

            var json =  response.ToJsonString(true);
            StringAssert.Contains(json, "titles");
        }

        [TestMethod]
        public async Task QueryProcedure()
        {
            var adapter = new MySqlAdapter("employees");
            var controller = new MySqlAdapterController();
            var response = await controller.GetObjectsAsync(adapter);

            var json = response.ToJsonString(true);
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
