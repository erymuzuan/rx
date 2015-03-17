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
        Verbose = 0,
        Info = 1,
        Log = 2,
        Debug = 3,
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
