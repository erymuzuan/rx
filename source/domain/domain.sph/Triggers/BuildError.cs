using System;
using System.Collections.Generic;
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
        public BuildError(string message, int line, string text)
        {
            this.Message = message;
            var code = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Take(line + 2)
                .ToArray();
            this.Code = $@"
{line - 2}:    {code[line - 3]}
{line - 1}:    {code[line - 2]}
{line}:    {code[line - 1]}
{line + 1}:    {code[line]}
{line + 2}:    {code[line + 1]}
";
            for (var i = line - 3; i >= 0; i--)
            {
                var id = Strings.RegexSingleValue(code[i].Trim(), "^//([A-Za-z0-9_]{0,25}):([A-Za-z0-9_]{0,25}):(?<id>.*?)$", "id");
                if (string.IsNullOrWhiteSpace(id)) continue;
                this.ItemWebId = id;
                break;
            }
        }
        public string Message { get; set; }
        public string Code { get; set; }
        public int Line { get; set; }
        public string ItemWebId { get; set; }
        public string FileName { get; set; }

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
            return obj.ItemWebId.GetHashCode() ^
                obj.Message.GetHashCode() ^
                obj.Line.GetHashCode() ^
                obj.FileName.GetHashCode();
        }
    }
}