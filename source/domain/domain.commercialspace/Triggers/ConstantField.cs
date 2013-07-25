using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ConstantField : Field
    {
        private object m_value;

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
                this.TypeName = value.AssemblyQualifiedName;
            }
        }

        public object Value
        {
            get { return m_value; }
            set
            {
                var val = string.Format("{0}", value);
                if (this.Type == typeof(int))
                    m_value = int.Parse(val);
                if (this.Type == typeof(DateTime))
                    m_value = DateTime.Parse(val);
                if (this.Type == typeof(decimal))
                    m_value = decimal.Parse(val);
                if (this.Type == typeof(bool))
                    m_value = bool.Parse(val);
                if (this.Type == typeof(string))
                    m_value = val;

                RaisePropertyChanged();
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