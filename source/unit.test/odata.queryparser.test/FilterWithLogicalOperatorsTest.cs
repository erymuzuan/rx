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
    public class FilterWithLogicalOperatorsTest
    {
        public FilterWithLogicalOperatorsTest(ITestOutputHelper console)
        {
            Console = console;

            var cache = new Mock<ICacheManager>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList<ISourceRepository>(SourceRepository);
            ObjectBuilder.AddCacheList(cache.Object);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        public ITestOutputHelper Console { get; }
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
        public void LogicalOperatorAnd()
        {
            const string TEXT = "$filter=Name eq 'Milk' and UnitPrice lt 2.55";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ftName = query.Filters.SingleOrDefault(x => x.Term == "Name");
            var ftPrice = query.Filters.SingleOrDefault(x => x.Term == "UnitPrice");

            Assert.NotNull(ftName);
            Assert.Equal(Operator.Eq, ftName.Operator);
            Assert.IsType<ConstantField>(ftName.Field);
            Assert.Equal("Milk", ftName.Field.GetValue(default));

            Assert.NotNull(ftPrice);
            Assert.Equal(Operator.Lt, ftPrice.Operator);
            Assert.IsType<ConstantField>(ftPrice.Field);
            Assert.Equal(2.55m, ftPrice.Field.GetValue(default));
        }

        [Fact]
        public void LogicalOperatorNot()
        {
            const string TEXT = "$filter=not (Name eq 'Milk')";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Lt, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("ilk", ft.Field.GetValue(default));
        }

        [Fact]
        public void LogicalOperatorOr()
        {
            const string TEXT = "$filter=Name eq 'Milk' or UnitPrice lt 2.55";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ftName = query.Filters.SingleOrDefault(x => x.Term == "Name");
            var ftPrice = query.Filters.SingleOrDefault(x => x.Term == "UnitPrice");

            Assert.NotNull(ftName);
            Assert.Equal(Operator.Eq, ftName.Operator);
            Assert.IsType<ConstantField>(ftName.Field);
            Assert.Equal("Milk", ftName.Field.GetValue(default));

            Assert.NotNull(ftPrice);
            Assert.Equal(Operator.Lt, ftPrice.Operator);
            Assert.IsType<ConstantField>(ftPrice.Field);
            Assert.Equal(2.55m, ftPrice.Field.GetValue(default));
        }
    }
}