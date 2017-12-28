using System.Linq;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class AggregateGroupByTest
    {
        [Fact]
        public void GroupByStateCount()
        {
            const string TEXT = "$apply=groupby((HomeAddress/State))";
            const string ENTITY = "Consignment";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var patientByState = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(patientByState);
            Assert.IsType<GroupByAggregate>(patientByState);
        }
        [Fact]
        public void GroupByStateAndAgeCount()
        {
            const string TEXT = "$apply=groupby((Name),aggregate(Sales/Amount with sum as Total))";
            const string ENTITY = "Consignment";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var patientByState = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(patientByState);
            Assert.IsType<GroupByAggregate>(patientByState);
        }
    }
}