using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using NUnit.Framework;

namespace sqlrepository.test
{
    [TestFixture]
    public class PersistenceTest
    {
        [Test]
        public void GetAggregatePropertyValue()
        {
            // var 
            var ed = new ReportDefinition
            {
                DataSource = new DataSource
                {
                    Query = "SELECT * FROM ME"
                }
            };
            var val = ed.MapColumnValue("DataSource.Query");
            Assert.AreEqual(ed.DataSource.Query, val);
        }
    }
}