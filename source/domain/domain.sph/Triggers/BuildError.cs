using System.Collections.Generic;

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