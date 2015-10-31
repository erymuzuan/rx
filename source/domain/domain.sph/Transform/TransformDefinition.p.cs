using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {

        [XmlIgnore]
        [JsonIgnore]
        public Type OutputType
        {
            get
            {
                return Strings.GetType(this.OutputTypeName);
            }
            set
            {
                this.OutputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type InputType
        {
            get
            {
                return Strings.GetType(this.InputTypeName);
            }
            set
            {
                this.InputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

       
    }
}
