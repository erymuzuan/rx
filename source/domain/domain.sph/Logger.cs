using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Logger : ILogger
    {
        public ObjectCollection<ILogger> Loggers { get; } = new ObjectCollection<ILogger>();

        public Task LogAsync(LogEntry entry)
        {
            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            var tasks = from logger in this.Loggers
                        select logger.LogAsync(entry);
            return Task.FromResult(0);

        }

        public void Log(LogEntry entry)
        {
            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;

            this.Loggers.ToList().ForEach(logger => logger.LogAsync(entry));
        }
    }
}