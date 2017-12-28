using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchQueryParsers;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.EsQueryParserTests
{
    public class FullTextQueryTest
    {
        private ITestOutputHelper Console { get; }

        public FullTextQueryTest(ITestOutputHelper console)
        {
            Console = console;
        }


        [Fact]
        public void QueryString()
        {
            string fullText = "this AND that OR thus";
            var text = $@"{{
    ""query"": {{
        ""query_string"" : {{
            ""default_field"" : ""*"",
            ""query"" : ""{fullText}""
        }}
    }}
}}";
            var entity = "Employee";
            var query = new QueryParser().Parse(text, entity);

            Assert.Single(query.Filters);
            var qs = query.Filters.Single();
            Assert.Equal(fullText, qs.Field.GetValue(default));
            Assert.Equal("*", qs.Term);
            Assert.Equal(Operator.FullText, qs.Operator);
            Assert.IsType<ConstantField>(qs.Field);
            Console.WriteLine(qs);
        }


    }
}