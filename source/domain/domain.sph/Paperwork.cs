using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public class Paperwork : Entity
    {
        public int PaperworkId { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
