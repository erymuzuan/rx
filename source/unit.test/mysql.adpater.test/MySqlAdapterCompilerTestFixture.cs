using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mysql.adpater.test
{
    [TestClass]
    public class MySqlAdapterCompilerTestFixture
    {
        [TestMethod]
        public async Task CompileOneTable()
        {
            var adapter = new MySqlAdapter
            {
                Name = "__MySqlEmployees",
                Database = "employees",
                Schema = "employees",
                Server = "localhost",
                UserId = "root",
                Password = "",
                Tables = new[]
                {
                    new AdapterTable
                    {
                        Name = "employees"
                    }
                }
            };
            await adapter.OpenAsync(true);
            var cr = await adapter.CompileAsync();
            Assert.IsTrue(cr.Result);
        }
    }
}
