using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Address", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Address : DomainObject
    {
        [XmlAttribute]
        public string Street { get; set; }
        [XmlAttribute]
        public string Postcode { get; set; }
        [XmlAttribute]
        public string State { get; set; }
        [XmlAttribute]
        public string City { get; set; }


    }
}
