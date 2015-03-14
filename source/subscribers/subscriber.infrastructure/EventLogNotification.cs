using System;
using System.Diagnostics;
using System.Text;

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

        public void WriteError(Exception exception, string message2)
        {
            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    this.WriteError(ie, "");
                }

            }
            while (null != exc)
            {
                message.AppendLine(exc.GetType().FullName);
                message.AppendLine(exc.Message);
                message.AppendLine(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            this.WriteError(message.ToString());
        }
    }
}