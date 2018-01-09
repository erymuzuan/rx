using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class ConsoleLogger : ILogger
    {
        public Severity TraceSwitch { get; set; } = Severity.Info;
        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)TraceSwitch) return;
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
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case Severity.Debug:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case Severity.Verbose:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
                //path.Substring(path.IndexOf("source\\") + "source\\".Length).Dump();
                Console.WriteLine(entry);
                if (TraceSwitch == Severity.Debug)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($@"   [{entry.CallerMemberName}]{entry.CallerFilePath}:{entry.CallerLineNumber}");
                    Console.WriteLine();
                }
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}