using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class VariableValue : DomainObject
    {
        [XmlIgnore]
        [JsonIgnore]
        public object Value { get; set; }
    }
}
