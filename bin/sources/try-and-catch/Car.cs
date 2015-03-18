using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Car", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Car : Vehicle
    {
        [XmlAttribute]
        public int Seating { get; set; }


    }
}
