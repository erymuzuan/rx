using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ILogger
    {
        void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application);
        Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application);
        Task LogAsync(Exception exception, IReadOnlyDictionary<string, object> properties);
        void Log(Exception exception, IReadOnlyDictionary<string, object> properties);
    }

    public enum Severity
    {
        Info,
        Warning,
        Error,
        Critical,
        Log
    }

    public enum LogEntry
    {
        Application,
        Security,
        Schedulers,
        Subscribers
    }
}
