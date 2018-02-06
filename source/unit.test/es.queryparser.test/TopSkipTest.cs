using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.EsQueryParserTests
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
            var entity = "Employee";
            var query = new QueryParser().Parse(text, entity);
            Assert.Equal(100, query.Skip);
            Assert.Equal(10, query.Size);
            Console.WriteLine(query);
        }
    }
}