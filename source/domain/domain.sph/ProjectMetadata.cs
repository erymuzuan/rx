using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class ProjectMetadata
    {
        public string TypeName { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
        public string Name { get; set; }
    }
}