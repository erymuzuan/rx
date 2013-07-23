using System;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ConstantField : Field
    {
        private object m_value;

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

        public object Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                RaisePropertyChanged();
                this.Type = value.GetType();
            }
        }


        public override object GetValue(Entity item)
        {
            var val = string.Format("{0}", this.Value);
            if (this.Type == typeof(int))
            {
                int f;
                if (int.TryParse(val, out f))
                    return f;
            }

            if (this.Type == typeof(DateTime))
            {
                DateTime f;
                if (DateTime.TryParse(val, out f))
                    return f;
            }
            if (this.Type == typeof(bool))
            {
                bool f;
                if (bool.TryParse(val, out f))
                    return f;
            }
            if (this.Type == typeof(decimal))
            {
                decimal f;
                if (decimal.TryParse(val, out f))
                    return f;
            }

            return this.Value;
        }
    }
}