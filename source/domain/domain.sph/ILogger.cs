using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ILogger
    {
        Task LogAsync(LogEntry entry);
        void Log(LogEntry entry);
    }

    public enum Severity
    {
        Debug = 0,
        Verbose = 1,
        Info = 2,
        Log = 3,
        Warning = 4,
        Error = 5,
        Critical = 6
    }

    public enum EventLog
    {
        Application,
        Security,
        Schedulers,
        Subscribers,
        WebServer,
        Elasticsearch,
        SqlRepository,
        SqlPersistence,
        Logger
    }
}
