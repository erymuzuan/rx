using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    public class IndexAliasTest
    {
        public ITestOutputHelper Console { get; }

        public IndexAliasTest(ITestOutputHelper console)
        {
            Console = console;
        }

        private class TestIndex : RepositoryWithNamingStrategy
        {
            public TestIndex() : base("http://localhost:9200", "Test")
            {
                
            }
            internal string GetAlias(IEnumerable<Filter> filters)
            {
                return base.GetIndexAlias(filters.ToArray());
            }
            internal string[] GetAliases(DateTime dateTime)
            {
                return base.GenerateAliasesName(dateTime);
            }
        }

    
        [Theory]
        [InlineData(IndexNamingStrategy.Daily, "2017-01-01", "2017-01-01", "Test_20170101")]// same day
        [InlineData(IndexNamingStrategy.Daily, "2017-01-03", "2017-01-05", "Test_2017W01")] // week
        [InlineData(IndexNamingStrategy.Daily, "2017-01-01", "2017-01-27", "Test_201701")] // same month
        [InlineData(IndexNamingStrategy.Daily, "2017-01-01", "2017-10-27", "Test_2017")] // year
        [InlineData(IndexNamingStrategy.Daily, "2017-01-01", null, "Test")] // year
        [InlineData(IndexNamingStrategy.Daily, null, "2017-10-27", "Test")] // year
        [InlineData(IndexNamingStrategy.Daily, null, null, "Test")] // year
        public void GetSearchAlias(IndexNamingStrategy strategy, string fromTex, string toText, string expecedAlias)
        {
            var index = new TestIndex
            {
                BaseIndexName = "Test",
                IndexNamingStrategy = strategy
            };
            var from = fromTex == null ? default : new Filter("CreatedDate", Operator.Ge, DateTime.Parse(fromTex));
            var to = toText == null ? default : new Filter("CreatedDate", Operator.Le, DateTime.Parse(toText).AddDays(1).AddSeconds(-1));

            var filters = new[] { from, to }.Where(x => null != x).ToArray();
            Assert.Equal(expecedAlias, index.GetAlias(filters));
        }


        [Theory]
        [InlineData("2017-01-03", "Test_2017W01", "Test_201701", "Test_2017")]
        [InlineData("2017-02-07", "Test_2017W06", "Test_201702", "Test_2017")]
        [InlineData("2017-04-18", "Test_2017W16", "Test_201704", "Test_2017")]
        [InlineData("2017-09-09", "Test_2017W36", "Test_201709", "Test_2017")]
        public void CreateAlias(string dateText, params string[] expectedAliases)
        {
            var index = new TestIndex
            {
                BaseIndexName = "Test",
                IndexNamingStrategy = IndexNamingStrategy.Daily
            };
            var aliases = index.GetAliases(DateTime.Parse(dateText));
            foreach (var expected in expectedAliases)
            {
                Console.WriteLine($"Expecting {expected} in [{expectedAliases.ToString(",")}]");
                Assert.Contains(expected, aliases);
            }
        }

    }
}