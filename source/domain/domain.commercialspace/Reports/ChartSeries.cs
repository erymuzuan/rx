using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ChartSeries : DomainObject
    {
        [XmlIgnore]
        [JsonIgnore]
        public decimal[] Values { get; set; }
    }
}