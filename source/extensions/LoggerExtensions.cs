using System.Runtime.CompilerServices;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Extensions
{
    internal static class LoggerExtension
    {
        public static void WritedVerbose(this ILogger logger, string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var verbose = new LogEntry
            {
                Severity = Severity.Verbose,
                Message = message,
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath

            };
            logger.Log(verbose);
        }

        public static void WriteDebug(this ILogger logger, string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var debug = new LogEntry
            {
                Severity = Severity.Debug,
                Message = message,
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath

            };
            logger.Log(debug);
        }
        public static void WriteError(this ILogger logger, string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var error = new LogEntry
            {
                Severity = Severity.Error,
                Message = message,
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath

            };
            logger.Log(error);
        }
        public static void WriteInfo(this ILogger logger, string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var info = new LogEntry
            {
                Severity = Severity.Info,
                Message = message,
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath

            };
            logger.Log(info);

        }
        public static void WriteWarning(this ILogger logger, string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var warning = new LogEntry
            {
                Severity = Severity.Warning,
                Message = message,
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath

            };
            logger.Log(warning);

        }
    }
}