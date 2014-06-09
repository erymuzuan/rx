using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Integrations.Adapters;
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
                Schema = "Person",
                Table = "Person",
                Name = "Sample SQL Adapter",
                Description = "A test"
               
            };
            await sql.OpenAsync();

            dynamic personType = await sql.CompileAsync();
            dynamic prs = Activator.CreateInstance(personType);
            Assert.IsNotNull(prs);

            prs.PersonType = "EM";
            prs.FirstName = Guid.NewGuid().ToString().Substring(0, 8);
            prs.LastName = "mustapa";
            prs.MiddleName = "bin";
            prs.Title = "Prog";
            prs.NameStyle = true;
            prs.ModifiedDate = DateTime.Now;
            prs.rowguid = Guid.NewGuid().ToString();
            prs.EmailPromotion = 1;
            prs.Suffix = "Mr";
     

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

        }
    }
}