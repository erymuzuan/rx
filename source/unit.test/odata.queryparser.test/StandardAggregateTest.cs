using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using odata.queryparser;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class StandardAggregateTest
    {
        public const string ENTITY = "Product";
        public ITestOutputHelper Console { get; }

        public StandardAggregateTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));

            var product = new EntityDefinition { Name = "Product", Id = "product", Plural = "Products", WebId = Strings.GenerateId() };
            product.AddSimpleMember<string>("WorkItemId");
            product.AddSimpleMember<int>("Age");
            var git = new MockSourceRepository();
            git.AddOrReplace(product);
            ObjectBuilder.AddCacheList<ISourceRepository>(git);
        }
        [Fact]
        public void ParseAggregateCountDistinct()
        {
            const string TEXT = "$apply=aggregate(WorkItemId with countdistinct as CountOfWorkItems)";
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var countOfWorkItems = query.Aggregates.SingleOrDefault(x => x.Name == "CountofWorkItems");
            Assert.NotNull(countOfWorkItems);
        }


        [Fact]
        public void ParseAggregateMax()
        {
            const string TEXT = "$apply=aggregate(WorkItemId with max as LatestItemId)";
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
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var latestId = query.Aggregates.SingleOrDefault(x => x.Name == "TotalAge");
            Assert.NotNull(latestId);
            Assert.IsType<SumAggregate>(latestId);
        }

    }
}