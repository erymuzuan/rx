using Xunit.Abstractions;

namespace Bespoke.Sph.QueryParserTests
{
    public class AggregatesTest
    {
        private ITestOutputHelper Console { get; }

        public AggregatesTest(ITestOutputHelper console)
        {
            Console = console;
        }
    }
}