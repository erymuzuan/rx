using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using sqlrepository.test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.SqlServer
{
    [Trait("Category", "Sql Server")]
    [Collection(SqlServerCollection.SQLSERVER_COLLECTION)]
    public class PredicateQueryOnServerTest
    {
        public SqlServerFixture Fixture { get; }
        private ITestOutputHelper Console { get; }

        private int Count(Func<Patient, bool> predicate)
        {
            return this.Fixture.Patients.Count(predicate);
        }

        public PredicateQueryOnServerTest(SqlServerFixture fixture, ITestOutputHelper console)
        {
            Fixture = fixture;
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        [Fact]
        public async Task IsNullInverse()
        {
            var married = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Wife.Name",Operator.IsNull, false)
            }));
            Assert.Equal(1, married.TotalRows);
        }
        [Fact]
        public async Task IsNotNullInverse()
        {
            var singles = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Wife.Name",Operator.IsNotNull, false)
            }));
            var expected = Count(x => x.Wife.Name == null);
            Assert.Equal(99, expected);
            Assert.Equal(expected, singles.TotalRows);
        }
        [Fact]
        public async Task IsNull()
        {
            var noWife = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Wife.Name",Operator.IsNull, true)
            }));
            Assert.Equal(Count(x => x.Wife.Name == null), noWife.TotalRows);
        }
        [Fact]
        public async Task IsNotNull()
        {
            var married = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Wife.Name",Operator.IsNotNull, true)
            }));
            Assert.Equal(Count(x => x.Wife.Name != null), married.TotalRows);
        }

        [Fact]
        public async Task StartWith()
        {
            var married = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Wife.Name",Operator.StartsWith, "K")
            }));
            var expected = Count(x => x.Wife.Name.ToEmptyString().StartsWith("K"));
            Assert.Equal(1, expected);
            Assert.Equal(expected, married.TotalRows);
        }

        [Fact]
        public async Task NotStartWith()
        {
            var notTans = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("FullName",Operator.NotStartsWith, "Tan")
            }));

            var expected = Count(x => !x.FullName.StartsWith("Tan", StringComparison.InvariantCultureIgnoreCase));
            Assert.Equal(98, expected);
            Assert.Equal(expected, notTans.TotalRows);
        }

        [Fact]
        public async Task EndsWith()
        {
            var notTans = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("FullName",Operator.EndsWith, "AN")
            }));

            var expected = Count(x => x.FullName.EndsWith("an", StringComparison.InvariantCultureIgnoreCase));
            Assert.Equal(8, expected);
            Assert.Equal(expected, notTans.TotalRows);
        }
        [Fact]
        public async Task NotEndsWith()
        {
            var lo = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("FullName",Operator.NotEndsWith, "AN")
            }));

            var expected = Count(x => !x.FullName.EndsWith("an", StringComparison.InvariantCultureIgnoreCase));
            Assert.Equal(92, expected);
            Assert.Equal(expected, lo.TotalRows);
        }


    }
}