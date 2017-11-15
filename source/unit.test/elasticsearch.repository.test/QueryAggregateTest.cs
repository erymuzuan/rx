using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    public class QueryAggregateTest
    {
        private ITestOutputHelper Console { get; }

        public QueryAggregateTest(ITestOutputHelper console)
        {
            Console = console;
        }

        /*https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket.html*/

        [Fact]
        public async Task TermsGroupByAggregate()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200","devv1");
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
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1_logs");
            var query = new QueryDsl();
           // query.Aggregates.Add(new DateRangeGroupByAggregate("severities", "severity", ranges:new DateRange[]{ new DateRange(DateTime.Today, DateTime.Today.AddMonths(1))}));
            var lo = await repos.SearchAsync(query);
            Console.WriteLine(lo);
            Assert.True(false, "TODO : get the buckets");
        }


    }
}