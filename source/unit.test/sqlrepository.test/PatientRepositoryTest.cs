using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using sqlrepository.test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [Trait("Category", "Sql Server")]
    [Collection(SqlServerCollection.SQLSERVER_COLLECTION)]
    public class PatientRepositoryTest
    {
        public SqlServerFixture Fixture { get; }
        private ITestOutputHelper Console { get; }

        public PatientRepositoryTest(SqlServerFixture fixture, ITestOutputHelper console)
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

        [Fact]
        public async Task ExecuteSimpleFilterQueryAsync()
        {
            var female = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }));
            Assert.Equal(33, female.TotalRows);
            var male = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Neq, "Female")
            }));
            Assert.Equal(67, male.TotalRows);

        }
        [Fact]
        public async Task ExecuteSimpleFilterIterateRowsQueryAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            });
            var female = await Fixture.Repository.SearchAsync(query);
            Assert.Equal(33, female.TotalRows);
            var male = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Neq, "Female")
            }));
            Assert.Equal(67, male.TotalRows);
            Assert.Equal(query.Size, male.ItemCollection.Count);

        }
        [Fact]
        public async Task ExecuteSimpleFilterWithPageQueryAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }, skip: 30);
            var female = await Fixture.Repository.SearchAsync(query);
            Assert.Equal(33, female.TotalRows);
            Assert.Equal(3, female.ItemCollection.Count);

        }


        [Fact]
        public async Task ExecuteQueryWithFieldsAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            var lo = await this.Fixture.Repository.SearchAsync(query);

            var reader = lo.Readers.OrderBy(x => Guid.NewGuid()).First();
            Assert.True(reader.ContainsKey("FullName"));
            Assert.True(reader.ContainsKey("Dob"));
            Assert.True(reader.ContainsKey("Age"));
            Assert.True(reader.ContainsKey("Id"));
            Assert.False(reader.ContainsKey("Religion"));

            Assert.IsType<int>(reader["Age"]);
            Assert.IsType<DateTime>(reader["Dob"]);
            Assert.Null(reader["Wife.Name"]);

            Assert.Equal(33, lo.TotalRows);
        }
        [Fact]
        public async Task MaxAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new MaxAggregate("LastModifiedDate", "ChangedDate"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<DateTime>("LastModifiedDate");
            Assert.Equal("2001-05-26T00:00:00", max.ToString("s"));
        }



    }
}