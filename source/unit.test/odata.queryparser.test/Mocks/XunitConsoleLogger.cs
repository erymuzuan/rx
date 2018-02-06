using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    internal class XunitConsoleLogger : ILogger
    {
        public XunitConsoleLogger(ITestOutputHelper console)
        {
            Console = console;
        }

        public ITestOutputHelper Console { get; }

        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            Console.WriteLine(entry.ToString());
        }
    }
}