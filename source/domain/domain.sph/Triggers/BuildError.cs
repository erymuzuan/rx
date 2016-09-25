using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class BuildError
    {
        public BuildError()
        {

        }
        public BuildError(string webid)
        {
            this.ItemWebId = webid;
        }

        public BuildError(string webid, string message)
        {
            this.ItemWebId = webid;
            this.Message = message;
        }

        public BuildError(CompilerError x)
        {
            this.Message = x.ErrorText;
            this.Line = x.Line;
            FileName = x.FileName;
            IsWarning = x.IsWarning;
            this.ErrorNumber = x.ErrorNumber;
            this.Column = x.Column;

            if (!string.IsNullOrWhiteSpace(x.FileName) && File.Exists(x.FileName))
            {
                this.Code = ExtractSnippets(File.ReadAllText(x.FileName), x.Line);
            }
        }

        public int Column { get; set; }
        public string ErrorNumber { get; set; }

        public BuildError(string message, int line, string text)
        {
            this.Message = message;
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
        public bool IsWarning { get; set; }

        public override string ToString()
        {
            return string.Format("{2}({0}) : {1}", this.Line, this.Message, this.FileName);
        }
    }

    public class BuildErrorComparer : IEqualityComparer<BuildError>
    {


        public bool Equals(BuildError x, BuildError y)
        {
            return x.ItemWebId == y.ItemWebId &&
                x.Message == y.Message &&
                x.Line == y.Line &&
                x.FileName == y.FileName;
        }

        public int GetHashCode(BuildError obj)
        {
            if (null == obj) return 0;
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