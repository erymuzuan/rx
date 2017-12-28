using System.Linq;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class StandardAggregateTest
    {
        [Fact]
        public void ParseAggregateCountDistinct()
        {
            const string TEXT = "$apply=aggregate(WorkItemId with countdistinct as CountOfWorkItems)";
            const string ENTITY = "Product";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var countOfWorkItems = query.Aggregates.SingleOrDefault(x => x.Name == "CountofWorkItems");
            Assert.NotNull(countOfWorkItems);
        }


        [Fact]
        public void ParseAggregateMax()
        {
            const string TEXT = "$apply=aggregate(WorkItemId with max as LatestItemId)";
            const string ENTITY = "Product";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "LatestItemId");
            Assert.NotNull(latestId);
            Assert.IsType<MaxAggregate>(latestId);
        }



        [Fact]
        public void ParseAggregateMin()
        {
            const string TEXT = "$apply=aggregate(Age with avg as AverageAge)";
            const string ENTITY = "Product";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "AverageAge");
            Assert.NotNull(latestId);
            Assert.IsType<AverageAggregate>(latestId);
        }
        [Fact]
        public void ParseAggregateSum()
        {
            const string TEXT = "$apply=aggregate(Age with sumb as TotalAge)";
            const string ENTITY = "Product";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(latestId);
            Assert.IsType<SumAggregate>(latestId);
        }

    }
}