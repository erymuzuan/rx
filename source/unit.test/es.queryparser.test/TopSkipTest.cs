using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class TopSkipTest
    {
        private ITestOutputHelper Console { get; }

        public TopSkipTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void Size()
        {
            var text = @"{
    ""from"" : 100, ""size"" : 10,
    ""query"" : {
        ""term"" : { ""user"" : ""kimchy"" }
    }
}";
            var query = new QueryParser().Parse(text);
            Assert.Equal(10, query.Skip);
            Assert.Equal(20, query.Size);
            Console.WriteLine(query);
        }
    }
}