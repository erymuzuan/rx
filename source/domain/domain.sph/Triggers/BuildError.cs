using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Domain
{
    public class BuildError
    {

        public BuildError(Diagnostic diagnostic)
        {
            this.Message = diagnostic.GetMessage();
            if (diagnostic.Location.IsInSource)
            {
                var span = diagnostic.Location.SourceSpan;
                var tree = diagnostic.Location.SourceTree.ToString();

                var code = new StringBuilder();
                code.AppendLine(tree.Substring(span.Start, span.Length));

                var lineNumber = tree.Take(span.Start).Count(c => c == '\n') + 1;
                var line = tree.Split(new[] { '\n' }, StringSplitOptions.None)
                    .ToArray()[lineNumber - 1];
                code.AppendLinf("{0} :  {1}", lineNumber, line);

                if (!string.IsNullOrWhiteSpace(diagnostic.Location.SourceTree.FilePath))
                {
                    this.FileName = diagnostic.Location.SourceTree.FilePath;
                }
                code.AppendLine("// " + diagnostic.Location.SourceTree.FilePath);

                this.Code = code.ToString();

                // get the item id
                this.ItemWebId = this.GetSourceError(lineNumber, tree.Split(new[] { '\n' }, StringSplitOptions.None));
            }

        }


        private string GetSourceError(int line, string[] sources)
        {

            var member = string.Empty;
            for (var i = 0; i < line; i++)
            {
                if (sources[i].Trim().StartsWith("//exec:"))
                    member = sources[i].Trim().Replace("//exec:", string.Empty);
            }
            return member;

        }


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
                x.Code == y.Code &&
                x.FileName == y.FileName;
        }

        public int GetHashCode(BuildError obj)
        {
            return obj.ItemWebId.GetHashCode() ^
                obj.Message.GetHashCode() ^
                obj.Code.GetHashCode() ^
                obj.FileName.GetHashCode();
        }
    }
}