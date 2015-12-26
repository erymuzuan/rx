using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class MethodArg : DomainObject
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

        public override string ToString()
        {
            var type = this.Type == null ? this.TypeName : this.Type.ToCSharp();
            var attribute = string.Join("\r\n", this.AttributeCollection);
            return $"{attribute}{type} {Name}";
        }

        public ObjectCollection<string> AttributeCollection { get; } = new ObjectCollection<string>();

        public object GetValue(RuleContext context)
        {
            return this.ValueProvider.GetValue(context);
        }
    }
}