using System;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class FieldChangeField : Field
    {
        [System.Xml.Serialization.XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.AssemblyQualifiedName;
            }
        }

    }
}