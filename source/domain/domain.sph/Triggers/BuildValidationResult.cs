﻿using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public class BuildValidationResult
    {
        public bool Result { get; set; }
        public string Uri { get; set; }
        public ObjectCollection<BuildDiagnostic> Errors { get; } = new ObjectCollection<BuildDiagnostic>();
        public ObjectCollection<BuildDiagnostic> Warnings { get; } = new ObjectCollection<BuildDiagnostic>();

        // prop
        public override string ToString()
        {
            var text = new StringBuilder();
            text.AppendLine($" {Errors.Count} errors and {Warnings.Count} warnings");
            var errors = from e in Errors
                         let line = e.Line == 0 ? "" : $"at {e.Line}"
                         select $"{e.Message} {line}";
            text.AppendLine(string.Join("\r\n", errors.ToArray()));
            return text.ToString();
        }
    }
}