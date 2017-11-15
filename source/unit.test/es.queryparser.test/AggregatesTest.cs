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

        private const string RequestLog = @"{
  ""query"": {
    ""range"": {
      ""time"": {
        ""from"": ""2015-01-14"",
        ""to"": ""2017-11-16""
      }
    }
  },
  ""aggs"": {
    ""path"": {
      ""terms"": {
        ""field"": ""request.path.raw""
      }
    }
  },
  ""fields"": [],
  ""from"": 0,
  ""size"": 1
}";

        private const string SumAgg = @"{
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
}";

        private const string DateHistogram = @"{
                ""query"": {
                    ""range"": {
                        ""time"": {
                            ""from"": ""2015-01-11"",
                            ""to"": ""2020-01-01""
                        }
                    }
                },
                ""aggs"": {
                    ""requests_over_time"": {
                        ""date_histogram"": {
                            ""field"": ""time"",
                            ""interval"": ""hour"",
                            ""offset"": ""+8h"",
                            ""format"": ""yyy-MM-dd:HH""
                        }
                    }
                },
                ""fields"": [],
                ""from"": 0,
                ""size"": 1
            }";

        private const string AggsAvgGradeAvgFieldGrade = @"{
    ""aggs"" : {
        ""avg_grade"" : { ""avg"" : { ""field"" : ""grade"" } }
    }
}";

        private const string AggsTypesCountValueCountFieldType = @"{
    ""aggs"" : {
        ""types_count"" : { ""value_count"" : { ""field"" : ""type"" } }
    }
}";

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
        [InlineData(AggsTypesCountValueCountFieldType, typeof(CountDistinctAggregate), "types_count", "type")]
        [InlineData(AggsAvgGradeAvgFieldGrade, typeof(AverageAggregate), "avg_grade", "grade")]
        [InlineData(SumAgg, typeof(SumAggregate), "hat_prices", "price")]    /* Sum */
        [InlineData(RequestLog, typeof(GroupByAggregate), "path", "request.path.raw")]
        [InlineData(DateHistogram, typeof(DateHistogramAggregate), "requests_over_time", "time")]
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