using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit.Abstractions;

namespace Bespoke.Sph.MessagingTests
{
    internal class XunitConsoleLogger : ILogger
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
            Console.WriteLine(entry.ToString());
        }
    }
}