using System.Xml.Serialization;

namespace Bespoke.CommercialSpace.Domain
{
    public partial class Change : DomainObject
    {
        [XmlAttribute]
        public string OldValue { get; set; }
        [XmlAttribute]
        public string NewValue { get; set; }
        [XmlAttribute]
        public string Field { get; set; }
        [XmlAttribute]
        public string Action { get; set; }
    }
}
