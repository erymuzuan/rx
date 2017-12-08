using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    // TODO : rename this to BuildDiagnostics, since we could have different Severity level : Error,Warning, Info etc
    public class BuildDiagnostic
    {
        public BuildDiagnostic()
        {

        }
        public BuildDiagnostic(string webid)
        {
            this.ItemWebId = webid;
        }

        public BuildDiagnostic(string webid, string message)
        {
            this.ItemWebId = webid;
            this.Message = message;
        }

        [Obsolete("Move to Roslyn")]
        public BuildDiagnostic(CompilerError x)
        {
            this.Message = x.ErrorText;
            this.Line = x.Line;
            FileName = x.FileName;
            Severity = x.IsWarning ? Severity.Warning : Severity.Error;
            this.ErrorNumber = x.ErrorNumber;
            this.Column = x.Column;

            if (!string.IsNullOrWhiteSpace(x.FileName) && File.Exists(x.FileName))
            {
                this.Code = ExtractSnippets(File.ReadAllText(x.FileName), x.Line);
            }
        }

        public int Column { get; set; }
        public string ErrorNumber { get; set; }

        public BuildDiagnostic(string message, int line, string text)
        {
            this.Message = message;
            if (!string.IsNullOrWhiteSpace(text))
                this.Code = ExtractSnippets(text, line);
        }

        private string ExtractSnippets(string text, int line)
        {
            var code = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
            .Take(line + 2)
            .Concat(new[] { "", "", "" })
            .ToArray();
            var snippet = $@"
{line - 2}:    {code[line - 3]}
{line - 1}:    {code[line - 2]}
{line}:    {code[line - 1]}
{line + 1}:    {code[line]}
{line + 2}:    {code[line + 1]}
";
            for (var i = line; i >= 0; i--)
            {
                var id = Strings.RegexSingleValue(code[i].Trim(), "^//([A-Za-z0-9_]{0,25}):([A-Za-z0-9_]{0,25}):(?<id>.*?)$", "id");
                if (string.IsNullOrWhiteSpace(id)) continue;
                this.ItemWebId = id;
                break;
            }

            return snippet;
        }
        public string Message { get; set; }
        public string Code { get; set; }
        public int Line { get; set; }
        public string ItemWebId { get; set; }
        public string FileName { get; set; }
        public Severity Severity { get; set; }

        public override string ToString()
        {
            return string.Format("{2}({0}) : {1}", this.Line, this.Message, this.FileName);
        }
    }

    public class BuildErrorComparer : IEqualityComparer<BuildDiagnostic>
    {


        public bool Equals(BuildDiagnostic x, BuildDiagnostic y)
        {
            if (null == x || null == y) return false;
            return x.ItemWebId == y.ItemWebId &&
                x.Message == y.Message &&
                x.Line == y.Line &&
                x.FileName == y.FileName;
        }

        public int GetHashCode(BuildDiagnostic obj)
        {
            try
            {
                return obj.ItemWebId.GetHashCode() ^
                       obj.Message.GetHashCode() ^
                       obj.Line.GetHashCode() ^
                       obj.FileName.GetHashCode();
            }
            catch
            {
                return 0;
            }
        }
    }
}