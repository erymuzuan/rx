using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class FileLogger : ILogger
    {
        public enum Interval
        {
            Hour,
            Day,
            WeekOfYear,
            MonthOfYear
        }
        private readonly string m_output;
        private readonly Interval m_rollingInterval;


        public FileLogger(string output, Interval rollingInterval = Interval.Day)
        {
            m_output = output;
            m_rollingInterval = rollingInterval;
        }
        public Severity TraceSwitch { get; set; } = Severity.Info;
        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        private string GetFileName()
        {
            var fileName = Path.GetFileNameWithoutExtension(m_output) ?? throw new InvalidOperationException(m_output + " is not a valid path");

            var folder = Path.GetDirectoryName(Path.GetFullPath(m_output));
            if (Path.IsPathRooted(m_output))
                folder = Path.GetDirectoryName(m_output);

            var timestamp = "";

            switch (m_rollingInterval)
            {
                case Interval.Hour:
                    timestamp = $"{DateTime.Now:yyyyMMdd-HH}";
                    break;
                case Interval.Day:
                    timestamp = $"{DateTime.Today:yyyyMMdd}";
                    break;
                case Interval.WeekOfYear:
                    var dfi = DateTimeFormatInfo.CurrentInfo ?? throw new InvalidOperationException("Cannot compute DateTimeFormatInfo.CurrentInfo");
                    var cal = dfi.Calendar;
                    timestamp = $"{DateTime.Today:yyyy}-{cal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek)}";
                    break;
                case Interval.MonthOfYear:
                    timestamp = $"{DateTime.Today:yyyyMM}";
                    break;
            }
            return $"{folder}\\{fileName}-{timestamp}{Path.GetExtension(m_output)}";
        }

        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)TraceSwitch) return;

            var message = $"{DateTime.Now:s}[{entry.Severity}]{entry}";
            var lines = new List<string> { message };
            if (TraceSwitch == Severity.Debug)
            {
                lines.Add($@"   [{entry.CallerMemberName}]{entry.CallerFilePath}:{entry.CallerLineNumber}");
            }
            File.AppendAllLines(GetFileName(), lines.ToArray());

        }
    }
}