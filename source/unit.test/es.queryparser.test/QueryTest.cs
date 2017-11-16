using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class QueryTest
    {
        private ITestOutputHelper Console { get; }

        public QueryTest(ITestOutputHelper console)
        {
            Console = console;
        }


        [Fact]
        public void Term()
        {
            var text = @"{
  ""query"": {
    ""term"" : { ""user"" : ""Kimchy"" } 
  }
}";
            var query = new QueryParser().Parse(text);

            Assert.Single(query.Filters);
            var term = query.Filters.Single();
            Assert.Equal("user", term.Term);
            Assert.Equal(Operator.Eq, term.Operator);
            Assert.IsType<ConstantField>(term.Field);
            Assert.Equal("Kimchy", term.Field.GetValue(default));
            Console.WriteLine(term);
        }

        [Fact]
        public void BoolWithMustMustNotAndShould()
        {
            var text = @"{
  ""query"": {
    ""bool"" : {
      ""must"" : {
        ""term"" : { ""user"" : ""kimchy"" }
      },
      ""filter"": {
        ""term"" : { ""tag"" : ""tech"" }
      },
      ""must_not"" : {
        ""range"" : {
          ""age"" : { ""gte"" : 10, ""lte"" : 20 }
        }
      },
      ""should"" : [
        { ""term"" : { ""tag"" : ""wow"" } },
        { ""term"" : { ""tag"" : ""elasticsearch"" } }
      ],
      ""minimum_should_match"" : 1,
      ""boost"" : 1.0
    }
  }
}";
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);
            Assert.Equal(5, query.Filters.Count);

            var user = query.Filters.Single(x => x.Term == "user");
            Assert.Equal("user", user.Term);
            Assert.Equal(Operator.Eq, user.Operator);
            Assert.IsType<ConstantField>(user.Field);
            Assert.Equal("kimchy", user.Field.GetValue(default));

            Assert.Single(query.Filters.OfType<BinaryOrFilter>());
            var or = query.Filters.OfType<BinaryOrFilter>().Single();
            Assert.Equal(2, or.Filters.Count);
        }


        [Fact]
        public void BoolMustAndMustNot()
        {
            var text = @"{
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""term"": {
            ""Race"":  ""Chinese""
          }
        },
        {
          ""term"": {
            ""HomeAddress.State"": ""Kelantan"" 
          }
        }
      ],
      ""must_not"": [
        {
          ""term"": {
            ""Gender"":  ""Male""
          }
        },
        {
          ""term"": {
            ""HomeAddress.District"": ""Pasir Mas"" 
          }
        }
      ]
    }
  }
}";
            var query = new QueryParser().Parse(text);

            Assert.Equal(4, query.Filters.Count);
            var chinese = query.Filters.First();
            Assert.Equal("Race", chinese.Term);
            Assert.IsType<ConstantField>(chinese.Field);
            Assert.Equal("Chinese", chinese.Field.GetValue(default));

            var gender = query.Filters.Single(x => x.Term == "Gender");
            Assert.Equal(Operator.Neq, gender.Operator);
        }

        [Fact]
        public void ShouldCompileToOr()
        {
            var text = @"{
  ""query"": {
    ""bool"": {
      ""should"": [
        {
          ""term"": {
            ""Race"":  ""Chinese""
          }
        },
        {
          ""term"": {
            ""HomeAddress.State"": ""Kelantan"" 
          }
        }
      ]
    }
  }
}";
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);

            Assert.Single(query.Filters.OfType<BinaryOrFilter>());
            var or = query.Filters.OfType<BinaryOrFilter>().First();
            var race = or.Filters.First(x => x.Term == "Race");
            Assert.Equal("Race", race.Term);
            Assert.IsType<ConstantField>(race.Field);
            Assert.Equal("Chinese", race.Field.GetValue(default));

            var state = or.Filters.Single(x => x.Term == "HomeAddress.State");
            Assert.Equal(Operator.Eq, state.Operator);
        }
    }
}