using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class FunctoidArg : DomainObject
    {
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public Functoid GetFunctoid(TransformDefinition map)
        {
            if (string.IsNullOrWhiteSpace(this.Functoid))
                return null;
            return map.FunctoidCollection.SingleOrDefault(x => x.WebId == this.Functoid);
        }
    }
}