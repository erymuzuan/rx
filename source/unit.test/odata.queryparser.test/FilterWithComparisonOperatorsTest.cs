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
    public class FilterWithComparisonOperatorsTest
    {
        public FilterWithComparisonOperatorsTest(ITestOutputHelper console)
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
        public void ComparisonOperatorEquals()
        {
            const string TEXT = "$filter=Name eq 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Eq, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }

        [Fact]
        public void ComparisonOperatorGreaterThan()
        {
            const string TEXT = "$filter=Name gt 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Gt, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }

        [Fact]
        public void ComparisonOperatorGreaterThanOrEqual()
        {
            const string TEXT = "$filter=Name ge 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Ge, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }

        [Fact]
        public void ComparisonOperatorHas()
        {
            const string TEXT = "$filter=style has Sales.Pattern'Yellow'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            //TODO: http://docs.oasis-open.org/odata/odata/v4.0/errata03/os/complete/part2-url-conventions/odata-v4.0-errata03-os-part2-url-conventions-complete.html#_Toc444868681
            var ft = query.Filters.SingleOrDefault(x => x.Term == "Style");
            Assert.NotNull(ft);
        }

        [Fact]
        public void ComparisonOperatorLessThan()
        {
            const string TEXT = "$filter=Name lt 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Lt, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }

        [Fact]
        public void ComparisonOperatorLessThanOrEqual()
        {
            const string TEXT = "$filter=Name le 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Le, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }

        [Fact]
        public void ComparisonOperatorNotEquals()
        {
            const string TEXT = "$filter=Name ne 'Milk'";
            const string ENTITY = "Product";

            var ed = CreateProductEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var ft = query.Filters.SingleOrDefault(x => x.Term == "Name");

            Assert.NotNull(ft);
            Assert.Equal(Operator.Neq, ft.Operator);
            Assert.IsType<ConstantField>(ft.Field);
            Assert.Equal("Milk", ft.Field.GetValue(default));
        }
    }
}