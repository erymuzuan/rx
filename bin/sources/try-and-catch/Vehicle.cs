using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Vehicle", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Vehicle : DomainObject
    {
        [XmlAttribute]
        public int Power { get; set; }
        [XmlAttribute]
        public decimal Cost { get; set; }
        [XmlAttribute]
        public string Name { get; set; }


    }
}
