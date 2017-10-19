using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Logger : ILogger
    {
        public Severity TraceSwitch { get; set; } = Severity.Info;
        public ObjectCollection<ILogger> Loggers { get; } = new ObjectCollection<ILogger>();

        public Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)TraceSwitch) return Task.FromResult(0);

            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            var tasks = from logger in this.Loggers
                        select logger.LogAsync(entry);
            Console.WriteLine($"executing {tasks.Count()} loggers");
            return Task.FromResult(0);

        }

        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)TraceSwitch) return;

            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            this.Loggers.ToList().ForEach(logger => logger.LogAsync(entry));
        }
    }
}