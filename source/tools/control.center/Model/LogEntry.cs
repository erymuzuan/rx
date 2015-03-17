using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bespoke.Sph.ControlCenter.Model
{
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
    public class LogEntry
    {

        public LogEntry([CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {

            this.CallerFilePath = filePath;
            this.CallerMemberName = memberName;
            this.CallerLineNumber = lineNumber;
        }

        public LogEntry(Exception exception, string[] otherInfo = null, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            this.CallerFilePath = filePath;
            this.CallerMemberName = memberName;
            this.CallerLineNumber = lineNumber;


            var keywords = new List<string>();
            var details = new StringBuilder();
            if (null != otherInfo)
            {
                details.AppendLine(" ============ OTHER INFO =============");
                foreach (var v in otherInfo)
                {
                    details.AppendLine(v);
                }

            }
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    keywords.Add(ie.GetType().FullName);
                    keywords.Add(ie.Message);
                    details.AppendLine(" ========================== ");
                    details.AppendLine(ie.GetType().FullName);
                    details.AppendLine(ie.Message);
                    details.AppendLine(ie.StackTrace);
                    details.AppendLine();
                    details.AppendLine();
                }

            }
            while (null != exc)
            {

                keywords.Add(exc.GetType().FullName);
                keywords.Add(exc.Message);

                details.AppendLine(" ========================== ");
                details.AppendLine(exc.GetType().FullName);
                details.AppendLine(exc.Message);
                details.AppendLine(exc.StackTrace);
                details.AppendLine();
                details.AppendLine();
                exc = exc.InnerException;
            }
            try
            {
                this.Severity = Severity.Error;
                this.Message = string.Format("{0} => {1}", exception.GetType().Name, exception.Message);
                this.Details = details.ToString();
                this.Time = DateTime.Now;
                this.Keywords = keywords.ToArray();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(details.ToString());
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }


        }

        public override string ToString()
        {
            return string.Format("{0}\r\n{1}", this.Message, this.Details);
        }

        public Severity Severity { get; set; }
        public EventLog Log { get; set; }
        public string Source { get; set; }
        public string Operation { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public string[] Keywords { get; set; }
        public string Details { get; set; }
        public string CallerFilePath { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        private readonly Dictionary<string, object> m_collection = new Dictionary<string, object>();

        public Dictionary<string, object> OtherInfo
        {
            get { return m_collection; }
        }
    }
}