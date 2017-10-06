using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    internal class FileLogger : ILogger
    {
        private readonly string m_output;

        public FileLogger(string output)
        {
            m_output = output;
            File.WriteAllText(m_output, "");
        }
        public Severity TraceSwitch { get; set; } = Severity.Info;
        public Task LogAsync(LogEntry entry)
        {
            Log(entry);
            return Task.FromResult(0);
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
            File.AppendAllLines(m_output, lines.ToArray());

        }
    }
}