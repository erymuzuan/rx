using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class SourceFieldsTest
    {
        private ITestOutputHelper Console { get; }

        public SourceFieldsTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void NoSourceFields()
        {
            var text = @"{
}";
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);
            Assert.Empty(query.Fields);

        }
        [Fact]
        public void Fields()
        {
            var text = @"{
    ""_source"" : [""FullName"", ""Mrn""]
}";
            var query = new QueryParser().Parse(text);
            Console.WriteLine(query);
            Assert.Equal(2, query.Fields.Count);

        }
    }
}