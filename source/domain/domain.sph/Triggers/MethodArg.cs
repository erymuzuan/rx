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
            var defaultValue = string.IsNullOrWhiteSpace(this.Default) ? "" : $" = {this.Default}";
            return $"{attribute}{type} {Name}{defaultValue}";
        }

        public ObjectCollection<string> AttributeCollection { get; } = new ObjectCollection<string>();
        public string Default { get; set; }

        public object GetValue(RuleContext context)
        {
            return this.ValueProvider.GetValue(context);
        }
    }
}