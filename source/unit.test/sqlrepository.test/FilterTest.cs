using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Tests.SqlServer.Extensions;
using sqlrepository.test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.SqlServer
{
    [Trait("Category", "Sql Server")]
    [Collection(SqlServerCollection.SQLSERVER_COLLECTION)]
    public class FilterTest
    {
        public SqlServerFixture Fixture { get; }
        private ITestOutputHelper Console { get; }

        public FilterTest(SqlServerFixture fixture, ITestOutputHelper console)
        {
            Fixture = fixture;
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }



        [Fact]
        public void OneEqTerm()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Male")
            });
            var sql = dsl.CompileToSql<Patient>().WriteObject(Console);
            Assert.Contains("[Gender] = 'Male'", sql);

        }
        [Fact]
        public void TwoEqTerms()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Age", Operator.Neq, 45),
                new Filter("Gender", Operator.Eq, "Female"),
                new Filter("Race", Operator.Eq, "Chinese")
            });
            var sql = dsl.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("[Age] <> 45 AND [Gender] = 'Female' AND [Race] = 'Chinese'", sql);

        }
        [Fact]
        public async Task FullTextAllField()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("*", Operator.FullText, "RANJIT OR LOW OR Islam")
            });
            var lo = await Fixture.Repository.SearchAsync(dsl);
            Assert.Equal(30, lo.TotalRows);

        }
        [Fact]
        public async Task FullTextFullName()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("FullName", Operator.FullText, "RANJIT OR LOW OR Islam")
            });
            var lo = await Fixture.Repository.SearchAsync(dsl);
            Assert.Equal(2, lo.TotalRows);

        }
        [Fact]
        public async Task FullTextFullNameAndHomeAddress()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("FullName", Operator.FullText, "RANJIT OR LOW OR Islam"),
                new Filter("HomeAddress.Street", Operator.FullText, "SS 2/72")
            });
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await Fixture.Repository.SearchAsync(dsl));
            Assert.Contains("more than 1 FullText", ex.Message);

        }
        [Fact]
        public void Range()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Age", Operator.Gt, 45),
                new Filter("Age", Operator.Lt, 65),
                new Filter("Gender", Operator.Eq, "Female"),
                new Filter("Race", Operator.Eq, "Chinese")
            });
            var sql = dsl.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("[Age] > 45", sql);
            Assert.Contains("[Age] < 65", sql);

        }

        [Fact]
        public void FilterInt32Term()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Age", Operator.Eq, 45)
            });
            var sql = dsl.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("[Age] = 45", sql);

        }

        [Fact]
        public void FilterDateTimeTerm()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Dob", Operator.Gt, new DateTime(1950,1,1))
            });
            var sql = dsl.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("[Dob] > '1950-01-01T00:00:00'", sql);

        }
    }
}
