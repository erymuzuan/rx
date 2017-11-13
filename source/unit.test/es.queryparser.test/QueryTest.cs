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

            Assert.Equal(1, query.Filters.Count);
            var term = query.Filters.Single();
            Assert.Equal("user", term.Term);
            Assert.Equal(Operator.Eq, term.Operator);
            Assert.IsType<ConstantField>(term.Field);
            Assert.Equal("Kimchy", term.Field.GetValue(default));
            Console.WriteLine(term.ToString());
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

            Assert.Equal(1, query.Filters.Count);
            var term = query.Filters.Single();
            Assert.Equal("user", term.Term);
            Assert.Equal(Operator.Eq, term.Operator);
            Assert.IsType<ConstantField>(term.Field);
            Assert.Equal("Kimchy", term.Field.GetValue(default));
            Console.WriteLine(term.ToString());
        }


        [Fact]
        public void BoolShould()
        {
            var text = @"{
  ""query"": {
    ""bool"": {
      ""should"": [
        {
          ""term"": {
            ""status"": {
              ""value"": ""urgent"",
              ""boost"": 2.0 
            }
          }
        },
        {
          ""term"": {
            ""status"": ""normal"" 
          }
        }
      ]
    }
  }
}";
            var query = new QueryParser().Parse(text);

            Assert.Equal(1, query.Filters.Count);
            var term = query.Filters.Single();
            Assert.Equal("user", term.Term);
            Assert.Equal(Operator.Eq, term.Operator);
            Assert.IsType<ConstantField>(term.Field);
            Assert.Equal("Kimchy", term.Field.GetValue(default));
        }
    }
}