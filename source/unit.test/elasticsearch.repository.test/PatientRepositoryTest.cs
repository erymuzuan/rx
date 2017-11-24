using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [Trait("Category", "Elasticsearch Server")]
    [Collection(ElasticsearchServerCollection.ELASTICSEARCH_COLLECTION)]
    public class PatientRepositoryTest
    {
        private ElasticsearchServerFixture Fixture { get; }
        private ITestOutputHelper Console { get; }
        private readonly JObject m_mapping;

        public PatientRepositoryTest(ElasticsearchServerFixture fixture, ITestOutputHelper console)
        {
            Fixture = fixture;
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
            // read JSON directly from a file
            using (var file = File.OpenText($@"{ConfigurationManager.Home}\..\source\unit.test\sample-data-patients\Patient.mapping"))
            using (var reader = new JsonTextReader(file))
            {
                m_mapping = (JObject) JToken.ReadFrom(reader);
            }
        }

        [Fact]
        public void OneEqTerm()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Male")
            });
            var query = dsl.CompileToElasticsearchQuery<Patient>(m_mapping);
            var json = JObject.Parse(query);
            Console.WriteLine(query);
            Assert.Single(json.SelectToken("$.filter.bool.must"));
            Assert.Empty(json.SelectToken("$.filter.bool.must_not"));
            Assert.Equal("Male", json.SelectToken("$.filter.bool.must[0].term.Gender").Value<string>());
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
            var query = dsl.CompileToElasticsearchQuery<Patient>(m_mapping);
            var json = Console.WriteJson(query);
            Assert.Equal(2, json.SelectToken("$.filter.bool.must").Count());
            Assert.Single(json.SelectToken("$.filter.bool.must_not"));
            Assert.Equal("Female", json.SelectToken("$.filter.bool.must[0].term.Gender").Value<string>());
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
        public async Task StartsWithFullName()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("FullName", Operator.StartsWith, "Mo")
            });
            var ex = await Assert.ThrowsAsync<NotSupportedException>(async () =>
                await Fixture.Repository.SearchAsync(dsl));
            Assert.Contains("FullName is analyzed", ex.Message);
        }

        [Fact]
        public async Task StartsHomeAddress()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("HomeAddress.State", Operator.StartsWith, "P")
            });
            var lo = await Fixture.Repository.SearchAsync(dsl);
            Assert.Equal(18, lo.TotalRows);
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
            var query = dsl.CompileToElasticsearchQuery<Patient>(m_mapping);
            var json = Console.WriteJson(query);
            Assert.Equal(3, json.SelectToken("$.filter.bool.must").Count());
            Assert.Empty(json.SelectToken("$.filter.bool.must_not"));
            Assert.Equal("Female", json.SelectToken("$.filter.bool.must[1].term.Gender").Value<string>());
        }

        [Fact]
        public void FilterInt32Term()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Age", Operator.Eq, 45)
            });
            var query = dsl.CompileToElasticsearchQuery<Patient>(m_mapping);
            var json = Console.WriteJson(query);
            Assert.Equal(45, json.SelectToken("$.filter.bool.must[0].term.Age").Value<int>());
        }


        [Theory]
        [InlineData("Age", Operator.Eq, 45)]
        [InlineData("Age", Operator.Ge, 45)]
        [InlineData("Dob", Operator.Gt, "DateTime'1950-01-01'")]
        [InlineData("Dob", Operator.Lt, "DateTime'1950-01-01'")]
        [InlineData("Dob", Operator.Le, "DateTime'1950-01-01'")]
        [InlineData("FullName", Operator.StartsWith, "Tan")]
        [InlineData("FullName", Operator.EndsWith, "Hock")]
        [InlineData("FullName", Operator.Substringof, "K")]
        [InlineData("Mrn", Operator.IsNull, true)] //missing
        [InlineData("Mrn", Operator.IsNotNull, false)] //missing
        public void FiltersMust(string term, Operator op, object value = null)
        {
            var text = $"{value}";
            if (text.StartsWith("DateTime"))
            {
                value = DateTime.Parse(text.Replace("DateTime", "").Replace("'", ""));
            }
            var filter = new Filter(term, op, value);
            var must = filter.IsMustFilter();

            Assert.True(must);
        }

        [Theory]
        [InlineData("Age", Operator.Neq, 45)]
        [InlineData("FullName", Operator.Neq, "Tan")]
        [InlineData("FullName", Operator.NotEndsWith, "Hock")]
        [InlineData("FullName", Operator.NotStartsWith, "Hock")]
        [InlineData("FullName", Operator.NotContains, "K")]
        [InlineData("Mrn", Operator.IsNull, false)] //missing
        [InlineData("Mrn", Operator.IsNotNull, true)] //missing
        public void FiltersMustNots(string term, Operator op, object value = null)
        {
            var filter = new Filter(term, op, value);
            var mustNot = filter.IsMustNotFilter();

            Assert.True(mustNot);
        }


        [Fact]
        public void FiltersMustNotNeq()
        {
            var age = new Filter("Age", Operator.Eq, 45);
            var gender = new Filter("Gender", Operator.Neq, "Female");
            var ageMust = age.IsMustFilter();
            var genderMustNot = gender.IsMustNotFilter();

            Assert.True(ageMust);
            Assert.True(genderMustNot);
        }


        [Fact]
        public void FilterDateTimeTerm()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Dob", Operator.Gt, new DateTime(1950, 1, 1))
            });
            var query = dsl.CompileToElasticsearchQuery<Patient>(m_mapping);
            var json = Console.WriteJson(query);
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].range.Dob.gt").Value<DateTime>(),
                new DateTime(1950, 1, 1));
        }

        [Fact]
        public async Task SimpleFilter()
        {
            var female = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }));
            Assert.Equal(33, female.TotalRows);
            var male = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Neq, "Female")
            }));
            Assert.Equal(67, male.TotalRows);
        }


        [Fact]
        public async Task Fields()
        {
            var repos = new ReadOnlyRepository<Patient>(Fixture.Url, Fixture.Index);
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] {new Sort {Direction = SortDirection.Desc, Path = "Mrn"}});
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            var lo = await repos.SearchAsync(query);

            var reader = lo.Readers.OrderBy(x => Guid.NewGuid()).First();
            Assert.True(reader.ContainsKey("FullName"));
            Assert.True(reader.ContainsKey("Dob"));
            Assert.True(reader.ContainsKey("Age"));
            Assert.True(reader.ContainsKey("Id"));
            Assert.False(reader.ContainsKey("Religion"));

            Assert.Null(reader["Wife.Name"]);
            Assert.IsType<DateTime>(reader["Dob"]);
            Assert.IsType<int>(reader["Age"]);

            Assert.Equal(33, lo.TotalRows);
        }

        [Fact]
        public async Task MaxAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] {new Sort {Direction = SortDirection.Desc, Path = "Mrn"}});
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new MaxAggregate("LastModifiedDate", "ChangedDate"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<DateTime>("LastModifiedDate");
            Assert.Equal("2001-05-26T00:00:00", max.ToString("s"));
        }
    }
}