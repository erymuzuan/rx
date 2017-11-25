using System.Diagnostics;

namespace sqlrepository.test.Models
{
    [DebuggerDisplay("{FullName}")]
    public class NextOfKin
    {
        public string FullName { get; set; }
        public string Relationship { get; set; }
    }
}