using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;

namespace Bespoke.Station.Domain
{
    public partial class LogEntry : Entity
    {
        private int m_logEntryId;

        [XmlAttribute]
        public int LogEntryId
        {
            get { return m_logEntryId; }
            set
            {
                m_logEntryId = value;
                RaisePropertyChanged();
            }
        }


        private async static Task Flush(int delay = 5000)
        {
            await Task.Delay(delay);
            /** */
            var context = new StationDataContext();
            var logs = m_entryCollection
                .Where(l => l.LogEntryId == 0)
                .Take(10).ToList();
            var i = 0;
            while (logs.Any())
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(logs.Cast<Entity>().ToArray());
                    await session.SubmitChanges();
                }
                logs = m_entryCollection.Take(10).Skip(10 * (++i)).ToList();
            }
            logs.Clear();

            await Flush();

        }

        private static readonly ObjectCollection<LogEntry> m_entryCollection = new ObjectCollection<LogEntry>();



        public async static void Add(LogEntry entry)
        {
            if (entry.DateTime == DateTime.MinValue) entry.DateTime = DateTime.Now;
            m_entryCollection.Add(entry);
            await Flush();
        }


        public async static Task<string> GetPublicIp()
        {
            try
            {
                var request = WebRequest.Create("http://checkip.dyndns.org/");
                string direction;
                using (var response = await request.GetResponseAsync())
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    direction = await stream.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(direction)) return null;
                }

                //Search for the ip in the html
                int first = direction.IndexOf("Address: ", StringComparison.Ordinal) + 9;
                int last = direction.LastIndexOf("</body>", StringComparison.Ordinal);
                direction = direction.Substring(first, last - first);
                return direction;
            }
            catch (WebException)
            {
                return "-";
            }
        }

        private static string m_currentIp;



        public async static void Add(string message, string source = "Application", LogLevel level = LogLevel.Information, bool flush = false)
        {
            if (string.IsNullOrWhiteSpace(m_currentIp))
                m_currentIp = await GetPublicIp();
            var entry = new LogEntry
                            {
                                Message = message,
                                Source = source,
                                Level = level,
                                DateTime = DateTime.Now,
                                Computer = Environment.MachineName,
                                IpAddress = m_currentIp,
                                UserName = "Test"
                            };

            m_entryCollection.Add(entry);
            if (flush)
               await Flush(10);
        }

        public static string Add(Exception exception, string source = "Application")
        {
            var message = new StringBuilder();
            LogException(exception, message);

            // get the loaded assembly
            Add(message.ToString(), level: LogLevel.Error, flush: true);
            return message.ToString();
        }

        private static void LogException(Exception exception, StringBuilder message)
        {
            var exc = exception;
            while (null != exc)
            {
                message.AppendLine();
                message.AppendLine();
                message.AppendLine("Type :" + exc.GetType());
                message.AppendLine("Message :" + exc.Message);
                message.AppendLine("Stack :" + exc.StackTrace);


                var aggregate = exc as AggregateException;
                if (null != aggregate)
                {
                    foreach (var x in aggregate.InnerExceptions)
                    {
                        LogException(x, message);
                    }
                }
                var loaderException = exception as System.Reflection.ReflectionTypeLoadException;
                if (null != loaderException)
                {
                    foreach (var lexc in loaderException.LoaderExceptions)
                    {
                        LogException(lexc, message);
                    }
                }

                exc = exc.InnerException;
            }
        }


    }
}