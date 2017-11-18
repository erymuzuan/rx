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
        public string Source { set; get; } = "Rx" + ConfigurationManager.ApplicationName;
        public string EventLogName { set; get; } = "Application";
        public Severity TraceSwitch { get; set; } = Severity.Info;

        public EventLogNotification()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, EventLogName);
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
            switch (entry.Severity)
            {
                case Severity.Debug:
                case Severity.Verbose:
                    break;
                case Severity.Info:
                case Severity.Log:
                    eLog.WriteEntry(message, EventLogEntryType.Information);
                    break;
                case Severity.Warning:
                    eLog.WriteEntry(message, EventLogEntryType.Warning);
                    break;
                case Severity.Error:
                case Severity.Critical:
                    eLog.WriteEntry(message, EventLogEntryType.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}