using System;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class AggregatesTest
    {
        private ITestOutputHelper Console { get; }

        public AggregatesTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void Max()
        {
            var text = @"{   ""aggs"" : {
        ""max_price"" : { ""max"" : { ""field"" : ""price"" } }
    }
    }";
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);
            Assert.Single(query.Aggregates.OfType<MaxAggregate>());
            var max = query.Aggregates.OfType<MaxAggregate>().Single();
            Assert.Equal("max_price", max.Name);
            Assert.Equal("price", max.Path);
        }

        [Theory]
        /*count*/
        [InlineData(@"{
    ""aggs"" : {
        ""types_count"" : { ""value_count"" : { ""field"" : ""type"" } }
    }
}", typeof(CountDistinctAggregate), "types_count", "type")]

        /*average */
        [InlineData(@"{
    ""aggs"" : {
        ""avg_grade"" : { ""avg"" : { ""field"" : ""grade"" } }
    }
}", typeof(AverageAggregate), "avg_grade", "grade")]

        /* Sum */
        [InlineData(@"{
    ""query"" : {
        ""constant_score"" : {
            ""filter"" : {
                ""match"" : { ""type"" : ""hat"" }
            }
        }
    },
    ""aggs"" : {
        ""hat_prices"" : { ""sum"" : { ""field"" : ""price"" } }
    }
}", typeof(SumAggregate), "hat_prices", "price")]
        public void Test(string text, Type expectedType, string expectedName, string expectedField)
        {
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);

            Assert.Single(query.Aggregates);
            var agg = query.Aggregates.Single();
            Assert.IsType(expectedType, agg);
            Assert.Equal(expectedName, agg.Name);
            Assert.Equal(expectedField, agg.Path);
        }
    }
}