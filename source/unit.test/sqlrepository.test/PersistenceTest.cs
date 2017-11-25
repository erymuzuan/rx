using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Xunit;

namespace Bespoke.Sph.Tests.SqlServer
{
    public class PersistenceTest
    {
        [Fact]
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
            Assert.Equal(ed.DataSource.Query, val);
        }
    }
}