using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ILogger
    {
        void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application);
        Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application);
        Task LogAsync(Exception exception);
    }

    public enum Severity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public enum LogEntry
    {
        Application,
        Security,
        Schedulers,
        Subscribers
    }


}
