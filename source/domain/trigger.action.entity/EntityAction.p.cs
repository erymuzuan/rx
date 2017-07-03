using System;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
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
        [XmlIgnore]
        [JsonIgnore]
        public Type EntityType
        {
            get
            {
                var type = Strings.GetType(this.OutboundEntity);
                if (null == type)
                {
                    var context = new SphDataContext();
                    var adapter = context.LoadOneFromSources<EntityDefinition>(x => x.Name == this.OutboundEntity || x.Id == this.OutboundEntity);
                    type = Strings.GetType(adapter.FullTypeName);
                }
                return type;
            }
            set
            {
                this.OutboundEntity = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public override bool UseAsync => true;

        public override bool UseCode => true;
    }
}
