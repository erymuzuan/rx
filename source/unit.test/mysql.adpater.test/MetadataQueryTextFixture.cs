using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mysql.adpater.test
{
    [TestClass]
    public class MetadataQueryTextFixture
    {
        [TestMethod]
        public  async Task QueryDatabases()
        {
            var adapter = new MySqlAdapter();
            var controller = new MySqlAdapterController();
            var response = await controller.GetDatabasesAsync(adapter);

            Assert.IsNotNull(response);
        }
    }
}
