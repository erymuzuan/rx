﻿namespace Bespoke.Sph.Domain
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

        public override string ToString()
        {
            return string.Format("line:{0} {1}", this.Line, this.Message);
        }
    }
}