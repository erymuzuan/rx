using System;
using System.Threading;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class ConsoleNotification : INotificationService
    {
        private readonly object m_lock = new object();



        private static string GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.None;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            return json;
        }

        private void SendMessage(LogEntry entry)
        {

            entry.Log = EventLog.Subscribers;
            entry.Time = DateTime.Now;
            entry.Computer = Environment.MachineName;
        }

        public void Write(string format, params object[] args)
        {
            try
            {
                Monitor.Enter(m_lock);
                var entry = new LogEntry
                {
                    Message = string.Format(format, args),
                    Severity = Severity.Info,
                    Time = DateTime.Now,
                    Log = EventLog.Subscribers
                };
                this.QueueUserWorkItem(SendMessage, entry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========== {0} : {1,12:hh:mm:ss.ff} ===========", "Infomation ", DateTime.Now);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
                Monitor.Exit(m_lock);
            }
        }

        public void WriteError(string format, params object[] args)
        {
            try
            {
                var entry = new LogEntry
                {
                    Message = "Error",
                    Severity = Severity.Error,
                    Details = string.Format(format, args),
                    Time = DateTime.Now
                };

                this.QueueUserWorkItem(SendMessage, entry);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public void WriteError(Exception exception, string message2)
        {
            lock (m_lock)
            {

                try
                {
                    var error = new LogEntry(exception);
                    this.QueueUserWorkItem(SendMessage, error);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.GetType().Name);
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(error.Details);
                    Console.WriteLine();
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }
    }
}
