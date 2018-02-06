using System;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Moq;
using odata.queryparser;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class StandardAggregateTest
    {
        public StandardAggregateTest(ITestOutputHelper console)
        {
            Console = console;

            var cache = new Mock<ICacheManager>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList<ISourceRepository>(SourceRepository);
            ObjectBuilder.AddCacheList(cache.Object);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
        }

        private ITestOutputHelper Console { get; }
        private MockSourceRepository SourceRepository { get; } = new MockSourceRepository();

        private static EntityDefinition CreateProductEntityDefinition(string name = "Product")
        {
            var ed = new EntityDefinition {Name = name, Plural = "Products", RecordName = "Code", Id = "product"};
            ed.AddSimpleMember<string>("Name");
            ed.AddSimpleMember<string>("Code");
            ed.AddSimpleMember<DateTime>("ValidUntil");
            ed.AddSimpleMember<decimal>("UnitPrice");
            ed.AddSimpleMember<int>("UnitsInStock");
            ed.AddSimpleMember<int>("UnitsOnOrder");
            ed.AddSimpleMember<bool>("Discontinued");

            return ed;
        }

        [Fact]
        public void ParseAggregateCountDistinct()
        {
            const string TEXT = "$apply=aggregate(Name with countdistinct as DistinctNames)";
            const string ENTITY = "Product";

            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            Assert.Single(query.Aggregates.OfType<CountDistinctAggregate>());
            var distinctNames = query.Aggregates.Single();
            Assert.Equal("Name", distinctNames.Path);
            Assert.Equal("DistinctNames", distinctNames.Name);
        }

        [Fact]
        public void ParseAggregateMax()
        {
            const string TEXT = "$apply=aggregate(UnitsOnOrder with max as MaxUnitsOnOrder)";
            const string ENTITY = "Product";

            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            Assert.Single(query.Aggregates.OfType<MaxAggregate>());
            var maxUnitsOnOrder = query.Aggregates.Single();
            Assert.Equal("UnitsOnOrder", maxUnitsOnOrder.Path);
            Assert.Equal("MaxUnitsOnOrder", maxUnitsOnOrder.Name);
        }

        [Fact]
        public void ParseAggregateMin()
        {
            const string TEXT = "$apply=aggregate(UnitsOnOrder with min as MinUnitsOnOrder)";
            const string ENTITY = "Product";

            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            Assert.Single(query.Aggregates.OfType<MinAggregate>());
            var minUnitsOnOrder = query.Aggregates.Single();
            Assert.Equal("UnitsOnOrder", minUnitsOnOrder.Path);
            Assert.Equal("MinUnitsOnOrder", minUnitsOnOrder.Name);
        }

        [Fact]
        public void ParseAggregateAverage()
        {
            const string TEXT = "$apply=aggregate(UnitPrice with average as AverageUnitPrice)";
            const string ENTITY = "Product";

            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            Assert.Single(query.Aggregates.OfType<AverageAggregate>());
            var averageUnitPrice = query.Aggregates.Single();
            Assert.Equal("UnitPrice", averageUnitPrice.Path);
            Assert.Equal("AverageUnitPrice", averageUnitPrice.Name);
        }

        [Fact]
        public void ParseAggregateSum()
        {
            const string TEXT = "$apply=aggregate(UnitPrice with sum as TotalUnitPrice)";
            const string ENTITY = "Product";

            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            Assert.Single(query.Aggregates.OfType<SumAggregate>());
            var totalUnitPrice = query.Aggregates.Single();
            Assert.Equal("UnitPrice", totalUnitPrice.Path);
            Assert.Equal("TotalUnitPrice", totalUnitPrice.Name);
        }
    }
}