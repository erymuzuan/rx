using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public class LogEntry
    {

        public LogEntry([CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {

            this.CallerFilePath = filePath;
            this.CallerMemberName = memberName;
            this.CallerLineNumber = lineNumber;
            this.Computer = Environment.MachineName;
            this.User = Environment.UserName;
        }

        public LogEntry(Exception exception, string[] otherInfo = null, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0) 
        {
            this.Exception = exception;
            this.CallerFilePath = filePath;
            this.CallerMemberName = memberName;
            this.CallerLineNumber = lineNumber;
            this.Computer = Environment.MachineName;
            this.User = Environment.UserName;

            var keywords = new List<string>();
            var details = new StringBuilder();
            details.AppendLine(" ============ OTHER INFO =============");
            details.JoinAndAppendLine(otherInfo, "\r\n");
            
            void Write(Exception ie)
            {
                keywords.Add(ie.GetType().FullName);
                keywords.Add(ie.Message);
                details.AppendLine(" ========================== ");
                details.AppendLine(ie.GetType().FullName);
                details.AppendLine(ie.Message);
                details.AppendLine(ie.StackTrace);
                details.AppendLine();
                details.AppendLine();

                switch (ie)
                {
                    case ReflectionTypeLoadException le:
                        le.LoaderExceptions.ToList().ForEach(Write);
                        break;
                    case AggregateException ae:
                        ae.InnerExceptions.ToList().ForEach(Write);
                        break;
                }
            }

            var exc = exception;
            while (null != exc)
            {
                Write(exc);
                exc = exc.InnerException;
            }
            
            this.Severity = Severity.Error;
            this.Message = $"{exception.GetType().Name} => {exception.Message}";
            this.Details = details.ToString();
            this.Time = DateTime.Now;
            this.Keywords = keywords.ToArray();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Details))
                return this.Message;

            return $"{this.Message}\r\n{this.Details}";
        }

        public Exception Exception { get; }
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

        public Dictionary<string, object> OtherInfo { get; } = new Dictionary<string, object>();
    }
}