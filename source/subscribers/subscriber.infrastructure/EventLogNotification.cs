using System;
using System.Diagnostics;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class EventLogNotification : INotificationService
    {
        public EventLogNotification()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, Log);
            }
        }
        public const string Source = "StationMsWorkerService";
        public const string Log = "Application";
        public void Write(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = Source };
            eLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void WriteError(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var eLog = new EventLog { Source = Source };
            eLog.WriteEntry(message, EventLogEntryType.Error);
        }
    }
}