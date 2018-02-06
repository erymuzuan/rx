using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.EsQueryParserTests
{
    public class AggregateGroupByTest
    {
        private ITestOutputHelper Console { get; }
        private const string Bucket = @"{
  ""sort"": [
    {
      ""time"": {
        ""order"": ""desc""
      }
    }
  ],
  ""aggs"": {
    ""category"": {
      ""terms"": {
        ""field"": ""severity"",
        ""size"": 0
      }
    }
  },
  ""fields"": [
    ""computer""
  ],
  ""from"": 0,
  ""size"": 20
}";

        private const string Entity = "Employee";

        public AggregateGroupByTest(ITestOutputHelper console)
        {
            Console = console;
        }
        /**/

        [Theory]
        [InlineData(Bucket, Entity)]
        public void GroupTerms(string text, string entity)
        {
            var query = new QueryParser().Parse(text, entity);
            Console.WriteLine(query);
            Assert.Single(query.Aggregates.OfType<GroupByAggregate>());
            var category = query.Aggregates.OfType<GroupByAggregate>().Single();
            Assert.Equal("category", category.Name);
            Assert.Equal("severity", category.Path);

        }
    }
}