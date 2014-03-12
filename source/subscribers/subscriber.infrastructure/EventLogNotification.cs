using System;
using System.Diagnostics;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class EventLogNotification : INotificationService
    {
        public const string SOURCE = "SPH";
        public const string LOG = "Application";

        public EventLogNotification()
        {
            if (!EventLog.SourceExists(SOURCE))
            {
                EventLog.CreateEventSource(SOURCE, LOG);
            }
        }

        public void Write(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = SOURCE };
            eLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void WriteError(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = SOURCE };
            eLog.WriteEntry(message, EventLogEntryType.Error);
        }
    }
}