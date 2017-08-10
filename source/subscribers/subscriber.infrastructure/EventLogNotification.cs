using System;
using System.Diagnostics;
using Bespoke.Sph.Domain;
using EventLog = System.Diagnostics.EventLog;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class EventLogNotification : INotificationService
    {
        public static string Source = "Rx" + ConfigurationManager.ApplicationName;
        public const string LOG = "Application";

        public EventLogNotification()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, LOG);
            }
        }

        public void Write(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = Source };
            eLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void WriteWarning(string message)
        {
            var eLog = new EventLog { Source = Source };
            eLog.WriteEntry(message, EventLogEntryType.Warning);
        }

        public void WriteError(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = Source };
            eLog.WriteEntry(message, EventLogEntryType.Error);
        }

        public void WriteError(Exception exception, string message2)
        {
            var message = new LogEntry(exception) { Message = message2 };
            this.WriteError(message.Details);
        }
    }
}