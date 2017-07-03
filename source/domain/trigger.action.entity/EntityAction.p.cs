using System;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Messaging
{
    public partial  class EntityAction
    {

        public string OutboundMap { get; set; }
        public string OutboundEntity{ get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Type OutboundMapType
        {
            get
            {
                return Strings.GetType(this.OutboundMap);
            }
            set
            {
                this.OutboundMap = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public override bool UseAsync => true;

        public override bool UseCode => true;
    }
}
