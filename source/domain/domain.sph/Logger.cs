using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Logger : ILogger
    {
        public ObjectCollection<ILogger> Loggers { get; } = new ObjectCollection<ILogger>();

        public async Task LogAsync(LogEntry entry)
        {
            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            var tasks = from logger in this.Loggers
                        select logger.LogAsync(entry);
            await Task.WhenAll(tasks);

        }

        public void Log(LogEntry entry)
        {
            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            this.Loggers.ToList().ForEach(logger => logger.LogAsync(entry));
        }
    }
}