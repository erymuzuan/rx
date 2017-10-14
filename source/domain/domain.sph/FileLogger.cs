using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Polly;

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
        private readonly string m_sizeLimit;
        private readonly object m_lock = new object();
        private long? m_longSize;

        private long LongSizeLimit
        {
            get
            {
                if (m_longSize.HasValue)
                    return m_longSize.Value;

                var factor = 1;
                if (m_sizeLimit.EndsWith("MB", StringComparison.InvariantCultureIgnoreCase))
                    factor = 6;
                if (m_sizeLimit.EndsWith("KB", StringComparison.InvariantCultureIgnoreCase))
                    factor = 3;
                if (m_sizeLimit.EndsWith("GB", StringComparison.InvariantCultureIgnoreCase))
                    factor = 9;

                if (double.TryParse(m_sizeLimit.Replace("KB", "")
                    .Replace("MB", "")
                    .Replace("GB", ""), out var length))
                {
                    m_longSize = Convert.ToInt64(length * Math.Pow(10, factor) * 1.024);
                    return m_longSize.Value;
                }
                // default to 1MB
                return Convert.ToInt64(1.024 * Math.Pow(10, 6));
            }
        }


        public FileLogger(string output, Interval rollingInterval = Interval.Day, string sizeLimit = "1MB")
        {
            m_output = output;
            m_rollingInterval = rollingInterval;
            m_sizeLimit = sizeLimit;
        }

        public Severity TraceSwitch { get; set; } = Severity.Info;

        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
        }

        private string GetFileName()
        {
            var fileName = Path.GetFileNameWithoutExtension(m_output) ??
                           throw new InvalidOperationException(m_output + " is not a valid path");

            var folder = Path.GetDirectoryName(Path.GetFullPath(m_output));
            if (Path.IsPathRooted(m_output))
                folder = Path.GetDirectoryName(m_output);

            string timestamp;
            var rolling = 1;

            switch (m_rollingInterval)
            {
                case Interval.Hour:
                    timestamp = $"{DateTime.Now:yyyyMMdd-HH}";
                    break;
                case Interval.Day:
                    timestamp = $"{DateTime.Today:yyyyMMdd}";
                    break;
                case Interval.WeekOfYear:
                    var dfi = DateTimeFormatInfo.CurrentInfo ??
                              throw new InvalidOperationException("Cannot compute DateTimeFormatInfo.CurrentInfo");
                    var cal = dfi.Calendar;
                    timestamp =
                        $"{DateTime.Today:yyyy}-{cal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek)}";
                    break;
                case Interval.MonthOfYear:
                    timestamp = $"{DateTime.Today:yyyyMM}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var extension = Path.GetExtension(m_output);
            var files = Directory.GetFiles(folder ?? throw new InvalidOperationException("folder cannot be null"),
                $"{fileName}-{timestamp}-*{extension}").OrderBy(x => x).ToList();
            if (files.Count == 0)
                return $"{folder}\\{fileName}-{timestamp}-{rolling:00}{extension}";


            var lastFile = new FileInfo(files.Last()).Length;
            if (lastFile >= LongSizeLimit)
                rolling = files.Count + 1;
            else
                rolling = files.Count;


            return $"{folder}\\{fileName}-{timestamp}-{rolling:00}{extension}";
        }

        public void Log(LogEntry entry)
        {
            if ((int) entry.Severity < (int) TraceSwitch) return;

            var header = $"{DateTime.Now:G} [{entry.Severity}]";
            var pad = new string(' ', header.Length);
            var messages = entry.ToEmptyString().Split(new[] {"\r\n", "\n", "\r"}, StringSplitOptions.None);
            var lines = new List<string> {$"{header} {messages.FirstOrDefault()}"};
            lines.AddRange(messages.Skip(1).Select(x => $"{pad} {x}"));
            if (TraceSwitch == Severity.Debug)
            {
                lines.Add($@"{pad}\t[{entry.CallerMemberName}]{Path.GetFileName(entry.CallerFilePath)}:{entry.CallerLineNumber}");
            }

            lock (m_lock)
            {
                Policy.Handle<IOException>()
                    .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(100 * Math.Pow(2, c)))
                    .Execute(() => File.AppendAllLines(GetFileName(), lines.ToArray()));
            }
        }
    }
}