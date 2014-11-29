using System;
using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mysql.adpater.test
{
    [TestClass]
    public class MetadataQueryTextFixture
    {
        const string ConnectionString = "Server=localhost;Database=test;Uid=root;Pwd=;";
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
                Database = "bphkuarters"
            };
            var controller = new MySqlAdapterController();
            var response = await controller.GetSchemasAsync(adapter);
            var content = (JsonContent)response.Content;

            var json = await content.ReadAsStringAsync();
            StringAssert.Contains(json,"bphkuarters");
            Assert.IsNotNull(response);
        }
        [TestMethod]
        public async Task QueryTables()
        {
            var adapter = new MySqlAdapter
            {
                ConnectionString = ConnectionString,
                Database = "bphkuarters"
            };
            var controller = new MySqlAdapterController();
            var response = await controller.GetTablesAsync(adapter);
            var content = (JsonContent)response.Content;

            var json = await content.ReadAsStringAsync();
            StringAssert.Contains(json, "occupant");
        }
    }
}
