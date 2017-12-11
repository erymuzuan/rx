using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class DiagnosticExtension
    {
        public static Severity FromDiagnostic(this DiagnosticSeverity diagnosticSeverity)
        {
            switch (diagnosticSeverity)
            {
                case DiagnosticSeverity.Hidden:
                    return Severity.Warning;
                case DiagnosticSeverity.Info:
                    return Severity.Info;
                case DiagnosticSeverity.Warning:
                    return Severity.Warning;
                case DiagnosticSeverity.Error:
                    return Severity.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diagnosticSeverity), diagnosticSeverity, null);
            }
        }
        public static BuildDiagnostic ToBuildError(this Diagnostic diagnostic)
        {
            var error = new BuildDiagnostic
            {
                Message = diagnostic.GetMessage(),
                Severity = diagnostic.Severity.FromDiagnostic()
            };
            if (diagnostic.Location.IsInSource)
            {
                var span = diagnostic.Location.SourceSpan;
                var tree = diagnostic.Location.SourceTree.ToString();

                var code = new StringBuilder();
                code.AppendLine(tree.Substring(span.Start, span.Length));

                var lineNumber = tree.Take(span.Start).Count(c => c == '\n') + 1;
                var line = tree.Split(new[] {'\n'}, StringSplitOptions.None)
                    .ToArray()[lineNumber - 1];
                code.AppendLinf("{0} :  {1}", lineNumber, line);

                if (!string.IsNullOrWhiteSpace(diagnostic.Location.SourceTree.FilePath))
                {
                    error.FileName = diagnostic.Location.SourceTree.FilePath;
                }
                code.AppendLine("// " + diagnostic.Location.SourceTree.FilePath);

                error.Code = code.ToString();
                error.Line = lineNumber;
                error.Message = diagnostic.Severity + ":" + diagnostic.GetMessage();

                // get the item id
                error.ItemWebId = GetSourceError(lineNumber, tree.Split(new[] {'\n'}, StringSplitOptions.None));
            }

            return error;
        }

        private static string GetSourceError(int line, string[] sources)
        {
            var member = string.Empty;
            for (var i = 0; i < line; i++)
            {
                if (sources[i].Trim().StartsWith("//exec:"))
                    member = sources[i].Trim().Replace("//exec:", string.Empty);
            }
            return member;
        }
    }
}