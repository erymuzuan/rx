using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_Sample2WithMultipleSourcesMapping_0
{
    [XmlType("Empty", Namespace = "")]
    public class Empty : DomainObject
    {
        [XmlAttribute]
        public string Name { get; set; }


    }
}
