using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.adapters
{
    [TestFixture]
    public class SqlAdapterApiTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Test]
        public async Task GenerateMetadataAsync()
        {
            var sql = new SqlServerAdapter
            {
                ConnectionString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=AdventureWorks;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False",
                Schema = "Purchasing",
                Table = "Vendor",
                Name = "Sample SQL Adapter",
                Description = "A test"
            };
            await sql.OpenAsync();

            dynamic metadata = await sql.CompileAsync();
            Assert.IsNotNull(metadata);

        }
    }
}