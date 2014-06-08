using System.Threading.Tasks;
using Bespoke.Sph.Integrations.Adapters;
using NUnit.Framework;

namespace oracle.adapter.test
{
    [TestFixture]
    public class OracleAdapterTest
    {
        [Test]
        public async Task TestOra()
        {
            var ora = new OracleAdapter
            {
                ConnectionString =  "Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=i90009638.cloudapp.net)(PORT=1521)))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));"
             + "User Id=SYSTEM;Password=gsxr750wt;",
                Table = "COUNTRIES",
                Name = "Hr Country",
                Description = "Ora HR Countries",
                Schema = "HR"
            };
            await ora.OpenAsync();

            dynamic metadata = await ora.CompileAsync();
            Assert.IsNotNull(metadata);
        }
    }
}
