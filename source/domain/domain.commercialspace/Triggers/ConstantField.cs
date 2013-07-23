using System;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ConstantField : Field
    {
        [XmlIgnore]
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
      
        public override object GetValue(Entity item)
        {
            if (this.Type == typeof (int))
            {
                int f;
                if (int.TryParse(this.Value, out f))
                    return f;
            }

            if (this.Type == typeof (DateTime))
            {
                DateTime f;
                if (DateTime.TryParse(this.Value, out f))
                    return f;
            }
            if (this.Type == typeof (bool))
            {
                bool f;
                if (bool.TryParse(this.Value, out f))
                    return f;
            }
            if (this.Type == typeof (decimal))
            {
                decimal f;
                if (decimal.TryParse(this.Value, out f))
                    return f;
            }

            return this.Value;
        }
    }
}