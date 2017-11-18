using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    class XunitConsoleLogger : ILogger
    {
        public ITestOutputHelper Console { get; }

        public XunitConsoleLogger(ITestOutputHelper console)
        {
            Console = console;
        }
        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            Console.WriteLine(entry);
        }
    }
    public class QueryAggregateTest
    {
        private ITestOutputHelper Console { get; }

        public QueryAggregateTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        /*https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket.html*/

        [Fact]
        public async Task TermsGroupByAggregate()
        {
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1");
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
            var repos = new ReadOnlyRepository<Patient>("http://localhost:9200", "devv1");
            var query = new QueryDsl();
            query.Aggregates.Add(new DateHistogramAggregate("date_of_births", "Dob", "year"));
            // ranges:new DateRange[]{ new DateRange(DateTime.Today, DateTime.Today.AddMonths(1))}));
            var lo = await repos.SearchAsync(query);
            Assert.Single(lo.Aggregates);

            Console.WriteLine(lo);
        }


    }
}