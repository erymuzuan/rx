using System;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocParameter : Member
    {
        public int? MaxLength { get; set; }
        public ParameterMode Mode { get; set; }
        public string SqlType { get; set; }
        public int Position { get; set; }
        public string TypeName { get; set; }
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
        public bool IsNullable { get; set; }
    }
}