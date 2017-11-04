using System;

namespace Bespoke.Sph.RxPs
{
    public class LogEntry
    {
        public string Hash { get; set; }
        public Severity Severity { get; set; }
        public EventLog Log { get; set; }
        public string Source { get; set; }
        public string Operation { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
        public string[] Keywords { get; set; }
        public string Details { get; set; }
        public string CallerFilePath { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
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
