using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    internal class ConsoleLogger : ILogger
    {
        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                switch (entry.Severity)
                {
                    case Severity.Critical:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case Severity.Error:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case Severity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case Severity.Info:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
                Console.WriteLine(entry);
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}