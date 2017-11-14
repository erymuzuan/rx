using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [Trait("Category", "Repository<T>")]
    [Collection("Repository")]
    public class PatientRepositoryTest
    {
        private ITestOutputHelper Console { get; }

        public PatientRepositoryTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void OneEqTerm()
        {
            var dsl = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Male")
            });
            var query = ((Patient)null).CompileToElasticsearchQueryDsl(dsl);
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
            var query = ((Patient)null).CompileToElasticsearchQueryDsl(dsl);
            var json = Console.WriteJson(query);
            Assert.Equal(2, json.SelectToken("$.filter.bool.must").Count());
            Assert.Single(json.SelectToken("$.filter.bool.must_not"));
            Assert.Equal("Female", json.SelectToken("$.filter.bool.must[0].term.Gender").Value<string>());

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
            var query = ((Patient)null).CompileToElasticsearchQueryDsl(dsl);
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
            var query = ((Patient)null).CompileToElasticsearchQueryDsl(dsl);
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
                new Filter("Dob", Operator.Gt, new DateTime(1950,1,1))
            });
            var query = ((Patient)null).CompileToElasticsearchQueryDsl(dsl);
            var json = Console.WriteJson(query);
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].range.Dob.gt").Value<DateTime>(), new DateTime(1950, 1, 1));

        }

        [Fact]
        public async Task SimpleFilter()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1");
            var lo = await repos.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }));
            Console.WriteLine(lo.ToString());

        }
        [Fact]
        public async Task Fields()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1");
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
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
        }
        [Fact]
        public async Task MaxAggregate()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1");
            var query = new QueryDsl(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new MaxAggregate("LastModifiedDate", "ChangedDate"));
            var lo = await repos.SearchAsync(query);


            var lastChanged = await GetLastModifiedDate(query.Aggregates.First());
            var max = lo.GetAggregateValue<DateTime>("LastModifiedDate");
            Assert.Equal(lastChanged.ToString("s"), max.ToString("s"));
        }
        [Fact]
        public async Task TermsGroupByAggregate()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1_logs");
            var query = new QueryDsl();
            query.Aggregates.Add(new GroupByAggregate("severities", "severity"));
            var lo = await repos.SearchAsync(query);
            Console.WriteLine(lo);
            Assert.True(false, "TODO : get the buckets");
        }

        private async Task<DateTime> GetLastModifiedDate(Aggregate agg)
        {
            var content = $@"
{{
   ""filter"": {{
      ""bool"": {{
         ""must"": [
            {{
               ""term"": {{
                  ""Gender"": ""Female""
               }}
            }}
         ],
         ""must_not"": []
      }}
   }},
   ""sort"": [
      {{
         ""Mrn"": {{
            ""order"": ""desc""
         }}
      }}
   ],
  ""size"": 0,
   ""aggs"": {{
      ""{agg.Name}"": {{
         ""max"": {{
             ""field"":""{agg.Path}""
         }}
      }}
   }}
}}";
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:9200") })
            {
                var response = await client.PostAsync("/devv1/patient/_search", new StringContent(content));
                var json = await Extensions.HttpClientExtensions.ReadContentAsJsonAsync(response);

                return json.SelectToken($"$.aggregations.{agg.Name}.value_as_string").Value<DateTime>();
            }
        }


    }

    [DebuggerDisplay("{Mrn}({FullName})")]
    public class Patient : Entity
    {
        public string Mrn { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public NextOfKin NextOfKin { get; set; }
    }

    [DebuggerDisplay("{FullName}")]
    public class NextOfKin
    {
        public string FullName { get; set; }
        public string Relationship { get; set; }
    }
}