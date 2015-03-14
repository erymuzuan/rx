using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Logger : ILogger
    {
        public void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            if (null == this.Loggers)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.Loggers) return;

            this.Loggers.ToList().ForEach(logger => logger.Log(operation, message, severity, entry));
        }
        public async Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            if (null == this.Loggers)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.Loggers) return;
            var tasks = from logger in this.Loggers
                        select logger.LogAsync(operation, message, severity, entry);
            await Task.WhenAll(tasks);
        }

        public async Task LogAsync(Exception exception, IReadOnlyDictionary<string, object> properties)
        {
            if (null == this.Loggers)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.Loggers) return;
            var tasks = from logger in this.Loggers
                        select logger.LogAsync(exception, properties);
            await Task.WhenAll(tasks);
        }

        public void Log(Exception exception, IReadOnlyDictionary<string, object> properties)
        {
            if (null == this.Loggers)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.Loggers) return;
            this.Loggers.ToList().ForEach(logger => logger.LogAsync(exception, properties));
        }

        [ImportMany(typeof(ILogger))]
        public ILogger[] Loggers { get; set; }
    }
}