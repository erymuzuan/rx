using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class DocumentField : Field
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
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public override object GetValue(RuleContext context)
        {
            var item = context.Item;
            if (string.IsNullOrWhiteSpace(this.Path)) 
                throw new InvalidOperationException("The Path property for " + this.Name + " cannot be null or empty");

            var script =  ObjectBuilder.GetObject<IScriptEngine>();
            var result = script.Evaluate<object, Entity>("item." + this.Path, item);
            return result;
        }

        public override string GenerateCode()
        {
            return string.Format("item.{0}", this.Path);
        }
    }
}