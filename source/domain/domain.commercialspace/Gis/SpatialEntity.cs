using System.Spatial;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public abstract class SpatialEntity : Entity
    {
        [XmlIgnore]
        [JsonIgnore]
        public Geography Path { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public string Wkt { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public string EncodedWkt { get; set; }
    }
}
