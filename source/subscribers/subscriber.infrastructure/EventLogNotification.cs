using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using EventLog = System.Diagnostics.EventLog;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class EventLogNotification : ILogger
    {
        public static string Source = "Rx" + ConfigurationManager.ApplicationName;
        public const string LOG = "Application";
        public Severity TraceSwitch { get; set; } = Severity.Info;

        public EventLogNotification()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, LOG);
            }
        }


        public Task LogAsync(LogEntry entry)
        {
            if ((int) entry.Severity >= (int) TraceSwitch)
                Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            if ((int) entry.Severity < (int) TraceSwitch) return;
            var message = entry.ToString();
            var eLog = new EventLog {Source = Source};
            eLog.WriteEntry(message, EventLogEntryType.Error);
        }
    }
}