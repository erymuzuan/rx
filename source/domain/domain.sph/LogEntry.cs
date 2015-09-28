using System;
using System.Collections.Generic;
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
        }

        public LogEntry(Exception exception, string[] otherInfo = null, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            this.Exception = exception;
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

            var rlex = exc as ReflectionTypeLoadException;
            if (null != rlex)
            {
                foreach (var lex in rlex.LoaderExceptions)
                {
                    keywords.Add(lex.GetType().FullName);
                    keywords.Add(lex.Message);
                    details.AppendLine(" ========================== ");
                    details.AppendLine(lex.GetType().FullName);
                    details.AppendLine(lex.Message);
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
                this.Message = $"{exception.GetType().Name} => {exception.Message}";
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
        public int Id { get; set; }
        public string[] Keywords { get; set; }
        public string Details { get; set; }
        public string CallerFilePath { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }

        public Dictionary<string, object> OtherInfo { get; } = new Dictionary<string, object>();
    }
}