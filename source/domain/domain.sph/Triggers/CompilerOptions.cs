using System.IO;

namespace Bespoke.Sph.Domain
{
    public class CompilerOptions
    {
        public bool IsDebug { get; set; }
        public bool IsVerbose { get; set; }
        public bool Emit { get; set; }
        public Stream Stream { get; set; }

     
     
    }
}