using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Bike", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Bike : Vehicle
    {
        [XmlAttribute]
        public bool IsLegal { get; set; }


    }
}
