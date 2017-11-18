using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class SortTest
    {
        private ITestOutputHelper Console { get; }

        public SortTest(ITestOutputHelper console)
        {
            Console = console;
        }


        [Fact]
        public void WithoutOrderSpecied()
        {
            var text = @"{
    ""sort"" : [
        {
            ""_geo_distance"" : {
                ""pin.location"" : {
                    ""lat"" : 40,
                    ""lon"" : -70
                },
                ""unit"" : ""km""
            }
        }
    ],
    ""query"" : {
        ""term"" : { ""user"" : ""kimchy"" }
    }
}";
            var parser = new QueryParser();
            var query = parser.Parse(text);

            var sort = query.Sorts.SingleOrDefault();
            Assert.NotNull(sort);
            Assert.Equal("_geo_distance", sort.Path);
            Assert.Equal(SortDirection.Asc, sort.Direction);

            Console.WriteLine(sort.ToString());
        }

        [Fact]
        public void WithOtherSortOptions()
        {
            var text = @"{
    ""sort"" : [
        {
            ""_geo_distance"" : {
                ""pin.location"" : {
                    ""lat"" : 40,
                    ""lon"" : -70
                },
                ""order"" : ""asc"",
                ""unit"" : ""km""
            }
        }
    ],
    ""query"" : {
        ""term"" : { ""user"" : ""kimchy"" }
    }
}";
            var parser = new QueryParser();
            var query = parser.Parse(text);

            var sort = query.Sorts.SingleOrDefault();
            Assert.NotNull(sort);
            Assert.Equal("_geo_distance", sort.Path);
            Assert.Equal(SortDirection.Asc, sort.Direction);

            Console.WriteLine(sort.ToString());
        }

        [Fact]
        public void MrnOrderAsc()
        {
            var text = @"{
                ""sort"": [
                 {
                     ""mrn"": {
                         ""order"": ""asc""
                     }
                 }
                ]
            }";
            var parser = new QueryParser();
            var query = parser.Parse(text);

            var sort = query.Sorts.SingleOrDefault();
            Assert.NotNull(sort);
            Assert.Equal("mrn", sort.Path);
            Assert.Equal(SortDirection.Asc, sort.Direction);

            Console.WriteLine(sort.ToString());
        }

        [Fact]
        public void MrnOrderAscMykadOrderDesc()
        {
            var text = @"{
                ""sort"": [
                    {
                       ""mrn"": {
                             ""order"": ""asc""
                         }
                    },
                    {
                     ""mykad"": {
                         ""order"": ""desc""
                     }

                    }
                ]
            }";
            var parser = new QueryParser();
            var query = parser.Parse(text);

            Assert.Equal(2, query.Sorts.Count);
            var mykad = query.Sorts.SingleOrDefault(x => x.Path == "mykad");
            Assert.NotNull(mykad);
            Assert.Equal(SortDirection.Desc, mykad.Direction);

            Console.WriteLine(mykad.ToString());
        }


        [Fact]
        public void TimeOrderDesc()
        {
            var text = @"{
                ""sort"": [
                 {
                     ""time"": {
                         ""order"": ""desc""
                     }
                 }
                ]
            }";
            var parser = new QueryParser();
            var query = parser.Parse(text);

            var sort = query.Sorts.SingleOrDefault();
            Assert.NotNull(sort);
            Assert.Equal("time", sort.Path);
            Assert.Equal(SortDirection.Desc, sort.Direction);

            Console.WriteLine(sort.ToString());

        }
    }
}
