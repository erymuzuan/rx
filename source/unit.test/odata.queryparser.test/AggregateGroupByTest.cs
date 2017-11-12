using System.Linq;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;

namespace domain.test.Queries
{
    public class AggregateGroupByTest
    {
        [Fact]
        public void GroupByStateCount()
        {
            var url = "$apply=groupby((HomeAddress/State))";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var patientByState = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(patientByState);
            Assert.IsType<GroupByAggregate>(patientByState);
        }
        [Fact]
        public void GroupByStateAndAgeCount()
        {
            var url = "$apply=groupby((Name),aggregate(Sales/Amount with sum as Total))";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var patientByState = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(patientByState);
            Assert.IsType<GroupByAggregate>(patientByState);
        }
    }
}