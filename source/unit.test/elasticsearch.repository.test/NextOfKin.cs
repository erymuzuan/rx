using System.Diagnostics;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [DebuggerDisplay("{FullName}")]
    public class NextOfKin
    {
        public string FullName { get; set; }
        public string Relationship { get; set; }
    }
}