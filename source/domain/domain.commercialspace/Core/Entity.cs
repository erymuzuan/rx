using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(Setting))]
    public abstract class Entity : DomainObject
    {
        [XmlAttribute]
        public string WebId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string CreatedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string ChangedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime ChangedDate { get; set; }


    }
}
