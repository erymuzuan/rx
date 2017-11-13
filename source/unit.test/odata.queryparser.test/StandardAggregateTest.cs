using System.Linq;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;

namespace Bespoke.Sph.QueryParserTests
{
    public class StandardAggregateTest
    {
        [Fact]
        public void ParseAggregateCountDistinct()
        {
            var url = "$apply=aggregate(WorkItemId with countdistinct as CountOfWorkItems)";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var countOfWorkItems = query.Aggregates.SingleOrDefault(x => x.Name == "CountofWorkItems");
            Assert.NotNull(countOfWorkItems);
        }
        
        
        [Fact]
        public void ParseAggregateMax()
        {
            var url = "$apply=aggregate(WorkItemId with max as LatestItemId)";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "LatestItemId");
            Assert.NotNull(latestId);
            Assert.IsType<MaxAggregate>(latestId);
        }
        
        
        
        [Fact]
        public void ParseAggregateMin()
        {
            var url = "$apply=aggregate(Age with avg as AverageAge)";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "AverageAge");
            Assert.NotNull(latestId);
            Assert.IsType<AverageAggregate>(latestId);
        }
        [Fact]
        public void ParseAggregateSum()
        {
            var url = "$apply=aggregate(Age with sumb as TotalAge)";
            var parser = new OdataQueryParser();
            var query = parser.Parse(url);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(latestId);
            Assert.IsType<SumAggregate>(latestId);
        }
        
    }
}