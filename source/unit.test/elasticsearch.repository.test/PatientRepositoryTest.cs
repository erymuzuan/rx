using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace elasticsearc.repository.test
{
    [Trait("Category", "Repository<T>")]
    [Collection("Repository")]
    public class PatientRepositoryTest
    {
        public ITestOutputHelper Console { get; }

        public PatientRepositoryTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void OneEqTerm()
        {
            var query = ((Patient)null).GenerateQueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Male")
            });
            var json = JObject.Parse(query);
            Console.WriteLine(query);
            Assert.Equal(1, json.SelectToken("$.filter.bool.must").Count());
            Assert.Equal(0, json.SelectToken("$.filter.bool.must_not").Count());
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].term.Gender").Value<string>(), "Male");

        }
        [Fact]
        public void TwoEqTerms()
        {
            var query = ((Patient)null).GenerateQueryDsl(new[]
            {
                new Filter("Age", Operator.Neq, 45),
                new Filter("Gender", Operator.Eq, "Female"),
                new Filter("Race", Operator.Eq, "Chinese")
            });
            var json = Console.WriteJson(query);
            Assert.Equal(2, json.SelectToken("$.filter.bool.must").Count());
            Assert.Equal(1, json.SelectToken("$.filter.bool.must_not").Count());
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].term.Gender").Value<string>(), "Female");

        }
        [Fact]
        public void Range()
        {
            var query = ((Patient)null).GenerateQueryDsl(new[]
            {
                new Filter("Age", Operator.Gt, 45),
                new Filter("Age", Operator.Lt, 65),
                new Filter("Gender", Operator.Eq, "Female"),
                new Filter("Race", Operator.Eq, "Chinese")
            });
            var json = Console.WriteJson(query);
            Assert.Equal(3, json.SelectToken("$.filter.bool.must").Count());
            Assert.Equal(0, json.SelectToken("$.filter.bool.must_not").Count());
            Assert.Equal(json.SelectToken("$.filter.bool.must[1].term.Gender").Value<string>(), "Female");

        }

        [Fact]
        public void FilterInt32Term()
        {
            var query = ((Patient)null).GenerateQueryDsl(new[]
            {
                new Filter("Age", Operator.Eq, 45)
            });
            var json = Console.WriteJson(query);
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].term.Age").Value<int>(), 45);

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
            var query = ((Patient)null).GenerateQueryDsl(new[]
            {
                new Filter("Dob", Operator.Gt, new DateTime(1950,1,1))
            });
            var json = Console.WriteJson(query);
            Assert.Equal(json.SelectToken("$.filter.bool.must[0].range.Dob.gt").Value<DateTime>(), new DateTime(1950, 1, 1));

        }

        [Fact]
        public async Task SimpleFilter()
        {
            var repos = new ReadonlyRepository<Patient>("http://localhost:9200", "devv1");
            var lo = await repos.SearchAsync(new[]
            {
                new Filter("Gender",Operator.Eq, "Female")
            });
            Console.WriteLine(lo.ToString());

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