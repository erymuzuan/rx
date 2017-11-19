using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [Trait("Category", "Elasticsearch Server")]
    [Collection(ElasticsearchServerCollection.ELASTICSEARCH_COLLECTION)]
    public class QueryAggregateTest
    {
        public ElasticsearchServerFixture Fixture { get; }
        private ITestOutputHelper Console { get; }

        public QueryAggregateTest(ElasticsearchServerFixture fixture, ITestOutputHelper console)
        {
            Fixture = fixture;
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        /*https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket.html*/

        [Fact]
        public async Task TermsGroupByAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl();
            query.Aggregates.Add(new GroupByAggregate("States", "HomeAddress.State"));
            var lo = await repos.SearchAsync(query);
            Console.WriteLine(lo);

            var state = lo.GetAggregateValue<Dictionary<string, int>>("States");
            Assert.NotEmpty(state);
            Console.WriteJson(state);
        }



        /*
         POST /sales/_search?size=0
{
    "aggs": {
        "range": {
            "date_range": {
                "field": "date",
                "format": "MM-yyy",
                "ranges": [
                    { "to": "now-10M/M" }, 
                    { "from": "now-10M/M" } 
                ]
            }
        }
    }
}
         
         */
        [Fact]
        public async Task DateRangeGroupByAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl();
            query.Aggregates.Add(new DateHistogramAggregate("date_of_births", "Dob", "year"));
            var lo = await repos.SearchAsync(query);
            Assert.Single(lo.Aggregates);

            Console.WriteLine(lo);
        }
        [Fact]
        public async Task DateRangeGroupByAggregateWithFilter()
        {
            var y1900 = new DateTime(1900, 01, 01);
            var y1930 = new DateTime(1930, 01, 01);
            const string NAME = "date_of_births";


            var repos = Fixture.Repository;
            var query = new QueryDsl();
            query.Filters.Add(new Filter("Dob", Operator.Ge, y1900));
            query.Filters.Add(new Filter("Dob", Operator.Le, y1930));

            query.Aggregates.Add(new DateHistogramAggregate(NAME, "Dob", "year"));
            var lo = await repos.SearchAsync(query);
            Assert.Single(lo.Aggregates);

            var results = lo.GetAggregateValue<Dictionary<DateTime, int>>(NAME);
            Assert.NotNull(results);
            foreach (var year in results.Keys)
            {
                Assert.InRange(year, y1900, y1930);
            }

            Console.WriteLine(lo);
        }


    }
}