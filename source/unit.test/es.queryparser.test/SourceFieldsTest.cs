using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.EsQueryParserTests
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
            var entity = "Employee";
            var query = new QueryParser().Parse(text, entity);
            Console.WriteLine(query);
            Assert.Empty(query.Fields);

        }
        [Fact]
        public void Sources()
        {
            var text = @"{
    ""_source"" : [""FullName"", ""Mrn""]
}";
            var entity = "Employee";
            var query = new QueryParser().Parse(text, entity);
            Console.WriteLine(query);
            Assert.Equal(2, query.Fields.Count);

        }
        [Fact]
        public void Fields()
        {
            var text = @"{
    ""fields"" : [""FullName"", ""Mrn""]
}";
            var entity = "Employee";
            var query = new QueryParser().Parse(text, entity);
            Console.WriteLine(query);
            Assert.Equal(2, query.Fields.Count);

        }
    }
}