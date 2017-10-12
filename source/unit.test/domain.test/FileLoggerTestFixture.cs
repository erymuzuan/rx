using System;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test
{
    [Trait("Logger", "File")]
    public class FileLoggerTestFixture
    {
        [Fact]
        public void Rolling()
        {
            var logger = new FileLogger(@"c:\temp\unit-test.log", FileLogger.Interval.Hour, "100KB")
            {
                TraceSwitch = Severity.Debug
            };

            for (var i = 0; i < 3 * Math.Pow(10, 3); i++)
            {
                logger.Log(new LogEntry
                {
                    Message = "The quick brown fox jumps over the lazy dog " + i,
                    Severity = Severity.Info
                });
            }
        }
    }
}